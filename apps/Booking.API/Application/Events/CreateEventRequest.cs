using System.ComponentModel.DataAnnotations;
using Booking.Domain;

namespace Booking.API.Application.Events;

/// <summary>
/// Datos para crear un evento nuevo junto con sus zonas de venta.
/// </summary>
public class CreateEventRequest
{
    /// <summary>Nombre del evento.</summary>
    /// <example>Recital de prueba</example>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción / bajada de marketing.</summary>
    public string? Description { get; set; }

    /// <summary>Ubicación en texto libre (nombre del lugar, dirección, ciudad).</summary>
    /// <example>Estadio Nacional, Lima</example>
    [Required]
    [MaxLength(255)]
    public string Location { get; set; } = string.Empty;

    /// <summary>Fecha y hora de inicio del evento (UTC).</summary>
    [Required]
    public DateTime EventDateTime { get; set; }

    /// <summary>URL del poster/banner.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Si el evento queda visible/a la venta al crearlo.</summary>
    public bool IsPublished { get; set; } = true;

    /// <summary>Zonas de venta (al menos una), con su tipo de entrada, precio y cupo.</summary>
    [MinLength(1, ErrorMessage = "El evento necesita al menos una zona de venta.")]
    public List<CreateEventZoneRequest> Zones { get; set; } = [];
}

/// <summary>
/// Zona de venta de un evento: un tipo de entrada con su precio y cupo.
/// </summary>
public class CreateEventZoneRequest
{
    /// <summary>Tipo de entrada: <c>General</c>, <c>VIP</c> o <c>Premium</c>.</summary>
    public TicketType TicketType { get; set; }

    /// <summary>Precio de la entrada en esta zona.</summary>
    /// <example>50.00</example>
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Price { get; set; }

    /// <summary>Cupo total de entradas a vender en esta zona.</summary>
    /// <example>100</example>
    [Range(1, int.MaxValue, ErrorMessage = "El cupo debe ser mayor a 0.")]
    public int Capacity { get; set; }
}
