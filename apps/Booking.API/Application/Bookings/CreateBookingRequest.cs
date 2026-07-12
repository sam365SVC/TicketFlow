using System.ComponentModel.DataAnnotations;

namespace Booking.API.Application.Bookings;

/// <summary>
/// Datos para crear una reserva: el evento y las zonas/cantidades elegidas.
/// </summary>
public class CreateBookingRequest
{
    /// <summary>Id del evento sobre el que se reserva.</summary>
    /// <example>1</example>
    [Required]
    public int EventId { get; set; }

    /// <summary>Zonas elegidas, cada una con su cantidad de entradas.</summary>
    [MinLength(1, ErrorMessage = "La reserva necesita al menos un item.")]
    public List<CreateBookingItemRequest> Items { get; set; } = [];
}

/// <summary>
/// Cantidad de entradas a reservar de una zona puntual.
/// </summary>
public class CreateBookingItemRequest
{
    /// <summary>Id de la zona de venta del evento.</summary>
    /// <example>1</example>
    [Required]
    public int EventZoneId { get; set; }

    /// <summary>Cantidad de entradas a reservar en esa zona.</summary>
    /// <example>2</example>
    [Range(1, short.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public short Quantity { get; set; }
}
