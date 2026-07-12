namespace Booking.Domain;

public class Payment
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Reservation Booking { get; set; } = null!;
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ExternalTransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
