namespace Booking.Domain;

public class Ticket
{
    public int Id { get; set; }
    public int BookingItemId { get; set; }
    public BookingItem BookingItem { get; set; } = null!;
    public int TicketStatusId { get; set; }
    public TicketStatus TicketStatus { get; set; } = null!;
    public string TicketCode { get; set; } = string.Empty;
    public DateTime? UsedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
