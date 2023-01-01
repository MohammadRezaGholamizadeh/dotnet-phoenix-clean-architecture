using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Phoenix.SharedConfiguration.Authorization;

namespace Phoenix.Infrastructure.Auth.Permissions;

internal class PermissionPolicyProvider
    : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public PermissionPolicyProvider(
           IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider =
            new DefaultAuthorizationPolicyProvider(options);
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return await FallbackPolicyProvider.GetDefaultPolicyAsync();
    }


    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(
            PhoenixClaims.Permission,
            StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return await Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return await FallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return await Task.FromResult<AuthorizationPolicy?>(null);

    }
}
