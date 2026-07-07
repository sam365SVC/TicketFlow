namespace Booking.API.Application.Auth;

/// <summary>
/// Credentials used to authenticate an existing user.
/// </summary>
public class LoginRequest
{
    /// <summary>Username registered previously.</summary>
    /// <example>jaimehuaycho</example>
    public string Username { get; set; } = string.Empty;

    /// <summary>Account password.</summary>
    public string Password { get; set; } = string.Empty;
}
