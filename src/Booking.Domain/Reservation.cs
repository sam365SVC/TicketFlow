namespace Booking.Domain;

public class Reservation
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public BookingStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
