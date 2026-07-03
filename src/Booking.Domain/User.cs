namespace Booking.Domain;

public class User
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public int? CreatedBy { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public short FailedAttempts { get; set; }
    public bool Active { get; set; }
    public bool RequiresPwdChange { get; set; }
    public DateTime? LockedUntil { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
