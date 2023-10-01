using Microsoft.AspNetCore.Authorization;

namespace CocktailApi;

public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
{
protected override Task HandleRequirementAsync(
    AuthorizationHandlerContext context,
    HasPermissionRequirement requirement) 
    {
        var claims = context.User.Claims;

        if (claims.Any(claim => claim.Type == "permissions" && claim.Value == requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}