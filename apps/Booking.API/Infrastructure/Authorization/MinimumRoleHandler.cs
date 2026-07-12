using Booking.API.Application.Authorization;
using Booking.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Booking.API.Infrastructure.Authorization;

public class MinimumRoleHandler : AuthorizationHandler<MinimumRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRoleRequirement requirement)
    {
        var roleClaim = context.User.FindFirst("role")?.Value;

        if (roleClaim is not null
            && Enum.TryParse<UserRole>(roleClaim, ignoreCase: true, out var userRole)
            && userRole >= requirement.MinimumRole)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
