using Booking.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Booking.API.Application.Authorization;

/// <summary>
/// Requisito de autorización: el usuario debe tener un rol igual o superior a <see cref="MinimumRole"/>
/// dentro de la jerarquía Customer &lt; Staff &lt; Admin. Un Admin cumple cualquier requisito de Staff,
/// y un Staff cumple cualquier requisito de Customer.
/// </summary>
public class MinimumRoleRequirement(UserRole minimumRole) : IAuthorizationRequirement
{
    public UserRole MinimumRole { get; } = minimumRole;
}
