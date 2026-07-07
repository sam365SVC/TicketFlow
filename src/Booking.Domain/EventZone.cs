namespace Booking.Domain;

public class EventZone
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public TicketType TicketType { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public int TicketsSold { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
}
