using System.IdentityModel.Tokens.Jwt;
using Booking.API.Application.Authorization;
using Booking.API.Application.Events;
using Booking.API.Infrastructure.ExceptionHandling;
using Booking.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    /// <summary>
    /// Alta y consulta de eventos y sus zonas de venta.
    /// </summary>
    [ApiController]
    [Route("api/events")]
    [Produces("application/json")]
    public class EventsController(
        CreateEventService createEventService,
        GetEventByIdService getEventByIdService,
        GetPublishedEventsService getPublishedEventsService) : ControllerBase
    {
        /// <summary>
        /// Crea un evento nuevo junto con sus zonas de venta. Requiere rol Staff o superior.
        /// </summary>
        /// <param name="request">Datos del evento y al menos una zona de venta.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>El evento creado, con sus zonas.</returns>
        /// <response code="200">Evento creado correctamente.</response>
        /// <response code="400">
        /// Datos inválidos. Dos casos posibles: (1) falta o está mal un campo del body (nombre vacío, precio negativo, etc.),
        /// en cuyo caso responde el formato estándar de validación de ASP.NET (<c>ValidationProblemDetails</c>: <c>errors</c> por campo);
        /// o (2) dos zonas repiten el mismo tipo de entrada, en cuyo caso responde el formato de error propio de la app
        /// (<c>{ message, error }</c> con <c>error = "DUPLICATE_EVENT_ZONE"</c>).
        /// </response>
        /// <response code="401">Falta el token o no es válido.</response>
        /// <response code="403">El usuario está logueado pero no tiene rol Staff o superior.</response>
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireStaff)]
        [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
            var @event = await createEventService.CreateAsync(request, userId, cancellationToken);

            return Ok(ToResponse(@event));
        }

        /// <summary>
        /// Lista los eventos publicados, ordenados por fecha. Endpoint público.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>Los eventos publicados con sus zonas de venta.</returns>
        /// <response code="200">Listado de eventos publicados.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<EventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPublished(CancellationToken cancellationToken)
        {
            var events = await getPublishedEventsService.GetAsync(cancellationToken);
            return Ok(events.Select(ToResponse));
        }

        /// <summary>
        /// Obtiene el detalle de un evento por id. Endpoint público.
        /// </summary>
        /// <param name="id">Id del evento.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>El evento y sus zonas de venta.</returns>
        /// <response code="200">Evento encontrado.</response>
        /// <response code="404">No existe un evento con ese id (<c>EVENT_NOT_FOUND</c>).</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var @event = await getEventByIdService.GetAsync(id, cancellationToken);
            return Ok(ToResponse(@event));
        }

        private static EventResponse ToResponse(Event @event) => new()
        {
            Id = @event.Id,
            Name = @event.Name,
            Description = @event.Description,
            Location = @event.Location,
            EventDateTime = @event.EventDateTime,
            ImageUrl = @event.ImageUrl,
            IsPublished = @event.IsPublished,
            Zones = @event.EventZones.Select(z => new EventZoneResponse
            {
                Id = z.Id,
                TicketType = z.TicketType.ToString(),
                Price = z.Price,
                Capacity = z.Capacity,
                TicketsSold = z.TicketsSold
            }).ToList()
        };
    }
}
