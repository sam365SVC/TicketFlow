namespace Booking.Domain;

public class TicketStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
