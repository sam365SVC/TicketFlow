namespace Booking.API.Application.Events;

/// <summary>
/// Datos públicos de un evento, con sus zonas de venta.
/// </summary>
public class EventResponse
{
    /// <summary>Id del evento.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Nombre del evento.</summary>
    /// <example>Recital de prueba</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción / bajada de marketing.</summary>
    public string? Description { get; set; }

    /// <summary>Ubicación en texto libre (nombre del lugar, dirección, ciudad).</summary>
    /// <example>Estadio Nacional, Lima</example>
    public string Location { get; set; } = string.Empty;

    /// <summary>Fecha y hora de inicio del evento (UTC).</summary>
    public DateTime EventDateTime { get; set; }

    /// <summary>URL del poster/banner, si tiene.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Si el evento está visible/a la venta.</summary>
    public bool IsPublished { get; set; }

    /// <summary>Zonas de venta del evento, con su disponibilidad actual.</summary>
    public List<EventZoneResponse> Zones { get; set; } = [];
}

/// <summary>
/// Zona de venta de un evento, con su disponibilidad actual.
/// </summary>
public class EventZoneResponse
{
    /// <summary>Id de la zona.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Tipo de entrada: <c>General</c>, <c>VIP</c> o <c>Premium</c>.</summary>
    /// <example>General</example>
    public string TicketType { get; set; } = string.Empty;

    /// <summary>Precio de la entrada en esta zona.</summary>
    /// <example>50.00</example>
    public decimal Price { get; set; }

    /// <summary>Cupo total de entradas a vender en esta zona.</summary>
    /// <example>100</example>
    public int Capacity { get; set; }

    /// <summary>Cuántas entradas de esta zona ya se vendieron.</summary>
    /// <example>0</example>
    public int TicketsSold { get; set; }
}
