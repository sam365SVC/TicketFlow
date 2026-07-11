using System.IdentityModel.Tokens.Jwt;
using Booking.API.Application.Bookings;
using Booking.API.Infrastructure.ExceptionHandling;
using Booking.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    /// <summary>
    /// Alta y consulta de reservas. La confirmación del pago (y todo lo que dispara: tickets,
    /// cupo vendido, notificación) no se expone acá — se resuelve internamente cuando se confirma el pago.
    /// </summary>
    [ApiController]
    [Route("api/bookings")]
    [Produces("application/json")]
    [Authorize]
    public class BookingsController(
        CreateBookingService createBookingService,
        GetBookingByIdService getBookingByIdService) : ControllerBase
    {
        /// <summary>
        /// Crea una reserva para el usuario autenticado: elige un evento y una o más zonas con cantidad.
        /// La reserva queda en estado <c>Pending</c> hasta que se confirme el pago.
        /// </summary>
        /// <param name="request">Evento y zonas/cantidades a reservar.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>La reserva creada, en estado <c>Pending</c>.</returns>
        /// <response code="200">Reserva creada correctamente.</response>
        /// <response code="400">
        /// Datos inválidos. Puede ser: campos faltantes (formato estándar de validación de ASP.NET),
        /// una zona que no pertenece al evento (<c>INVALID_EVENT_ZONE</c>), o no hay cupo suficiente
        /// en alguna zona (<c>INSUFFICIENT_CAPACITY</c>).
        /// </response>
        /// <response code="401">Falta el token o no es válido.</response>
        /// <response code="404">No existe un evento con ese id (<c>EVENT_NOT_FOUND</c>).</response>
        [HttpPost]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
            var reservation = await createBookingService.CreateAsync(request, userId, cancellationToken);

            return Ok(ToResponse(reservation));
        }

        /// <summary>
        /// Obtiene el detalle de una reserva por id. Solo puede verla su dueño, o un usuario con rol Staff o superior.
        /// </summary>
        /// <param name="id">Id de la reserva.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>La reserva y sus items.</returns>
        /// <response code="200">Reserva encontrada.</response>
        /// <response code="401">Falta el token o no es válido.</response>
        /// <response code="404">
        /// No existe una reserva con ese id, o existe pero no le pertenece al usuario autenticado
        /// (<c>BOOKING_NOT_FOUND</c> en ambos casos, para no revelar si la reserva existe).
        /// </response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
            var role = Enum.Parse<UserRole>(User.FindFirst("role")!.Value, ignoreCase: true);

            var reservation = await getBookingByIdService.GetAsync(id, userId, role, cancellationToken);

            return Ok(ToResponse(reservation));
        }

        private static BookingResponse ToResponse(Reservation reservation) => new()
        {
            Id = reservation.Id,
            EventId = reservation.EventId,
            EventName = reservation.Event.Name,
            Status = reservation.Status.ToString(),
            TotalAmount = reservation.TotalAmount,
            CreatedAt = reservation.CreatedAt,
            Items = reservation.BookingItems.Select(i => new BookingItemResponse
            {
                Id = i.Id,
                EventZoneId = i.EventZoneId,
                TicketType = i.EventZone.TicketType.ToString(),
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.UnitPrice * i.Quantity
            }).ToList()
        };
    }
}
