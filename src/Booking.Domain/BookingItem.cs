namespace Booking.Domain;

public class BookingItem
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Reservation Booking { get; set; } = null!;
    public int EventZoneId { get; set; }
    public EventZone EventZone { get; set; } = null!;
    public short Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
