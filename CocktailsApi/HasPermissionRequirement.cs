using Microsoft.AspNetCore.Authorization;

namespace CocktailApi;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public HasPermissionRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}