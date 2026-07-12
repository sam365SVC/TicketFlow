namespace Booking.API.Application.Authorization;

/// <summary>
/// Nombres de las políticas de autorización jerárquica registradas en <c>Program.cs</c>.
/// Usar con <c>[Authorize(Policy = AuthorizationPolicies.RequireStaff)]</c> en los controladores.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>Permite Staff o Admin.</summary>
    public const string RequireStaff = "RequireStaff";

    /// <summary>Permite únicamente Admin.</summary>
    public const string RequireAdmin = "RequireAdmin";
}
