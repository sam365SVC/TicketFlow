namespace Booking.API.Application.Bookings;

/// <summary>
/// Datos de una reserva, con sus items.
/// </summary>
public class BookingResponse
{
    /// <summary>Id de la reserva.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Id del evento reservado.</summary>
    /// <example>1</example>
    public int EventId { get; set; }

    /// <summary>Nombre del evento reservado.</summary>
    /// <example>Recital de prueba</example>
    public string EventName { get; set; } = string.Empty;

    /// <summary>Estado de la reserva: <c>Pending</c>, <c>Confirmed</c>, <c>Failed</c> o <c>Cancelled</c>.</summary>
    /// <example>Pending</example>
    public string Status { get; set; } = string.Empty;

    /// <summary>Monto total de la reserva (suma de todos los items).</summary>
    /// <example>220.00</example>
    public decimal TotalAmount { get; set; }

    /// <summary>Fecha de creación de la reserva (UTC).</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Items reservados, uno por cada zona elegida.</summary>
    public List<BookingItemResponse> Items { get; set; } = [];
}

/// <summary>
/// Item de una reserva: una zona con su cantidad y precio congelado al momento de reservar.
/// </summary>
public class BookingItemResponse
{
    /// <summary>Id del item.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Id de la zona de venta reservada.</summary>
    /// <example>1</example>
    public int EventZoneId { get; set; }

    /// <summary>Tipo de entrada de la zona: <c>General</c>, <c>VIP</c> o <c>Premium</c>.</summary>
    /// <example>General</example>
    public string TicketType { get; set; } = string.Empty;

    /// <summary>Cantidad de entradas reservadas en esta zona.</summary>
    /// <example>2</example>
    public short Quantity { get; set; }

    /// <summary>Precio unitario congelado al momento de reservar.</summary>
    /// <example>50.00</example>
    public decimal UnitPrice { get; set; }

    /// <summary>Subtotal del item (<c>UnitPrice * Quantity</c>).</summary>
    /// <example>100.00</example>
    public decimal Subtotal { get; set; }
}
