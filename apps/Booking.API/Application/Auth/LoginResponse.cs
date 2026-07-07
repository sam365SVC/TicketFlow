namespace Booking.API.Application.Auth;

/// <summary>
/// JWT access token issued after a successful login.
/// </summary>
public class LoginResponse
{
    /// <summary>Signed JWT. Send it as <c>Authorization: Bearer &lt;token&gt;</c> on subsequent requests.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>UTC date/time when the token stops being valid.</summary>
    public DateTime ExpiresAt { get; set; }
}
