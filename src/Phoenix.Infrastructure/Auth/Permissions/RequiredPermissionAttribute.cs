using Microsoft.AspNetCore.Authorization;
using Phoenix.SharedConfiguration.Authorization;

namespace Phoenix.Infrastructure.Auth.Permissions;

public class RequiredPermissionAttribute
             : AuthorizeAttribute
{
    public RequiredPermissionAttribute(string action, string resource)
    {
        Policy = new PhoenixPermission("Policy", action, resource).Name;
    }
}
