namespace Booking.Domain;

public class PaymentStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
