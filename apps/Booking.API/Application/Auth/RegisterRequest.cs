namespace Booking.API.Application.Auth;

/// <summary>
/// Data required to create a new user account.
/// </summary>
public class RegisterRequest
{
    /// <summary>Full name of the user.</summary>
    /// <example>Jaime Huaycho</example>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Unique username used to log in.</summary>
    /// <example>jaimehuaycho</example>
    public string Username { get; set; } = string.Empty;

    /// <summary>Unique email address.</summary>
    /// <example>jaime@ticketflow.local</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>Plain-text password (never stored as-is; it gets hashed with BCrypt).</summary>
    public string Password { get; set; } = string.Empty;
}
