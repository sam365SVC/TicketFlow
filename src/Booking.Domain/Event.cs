namespace Booking.Domain;

public class Event
{
    public int Id { get; set; }
    public int CreatedBy { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<EventZone> EventZones { get; set; } = new List<EventZone>();
}
