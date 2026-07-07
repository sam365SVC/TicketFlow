namespace Booking.API.Application.Auth;

/// <summary>
/// Public data of a newly registered user (never includes the password hash).
/// </summary>
public class RegisterResponse
{
    /// <summary>Unique identifier assigned to the new user.</summary>
    public int Id { get; set; }

    /// <summary>Username the user will use to log in.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Email address associated with the account.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Role assigned to the account. New registrations are always <c>Customer</c>.</summary>
    public string Role { get; set; } = string.Empty;
}
