using Microsoft.EntityFrameworkCore;
using Phoenix.Application.Common.Exceptions;
using Phoenix.SharedConfiguration.Authorization;
using Phoenix.SharedConfiguration.Common.ApplicationUsers;

namespace Phoenix.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new UnauthorizedException("Authentication Failed.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
            .Where(r => userRoles.Contains(r.Name))
            .ToListAsync(cancellationToken))
        {
            permissions.AddRange(await _db.Set<ApplicationRoleClaim>()
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == PhoenixClaims.Permission)
                .Select(rc => rc.ClaimValue)
                .ToListAsync(cancellationToken));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
    {
        //var permissions = await _cache.GetOrSetAsync(
        //    _cacheKeys.GetCacheKey(PhoenixClaims.Permission, userId),
        //    () => GetPermissionsAsync(userId, cancellationToken),
        //    cancellationToken: cancellationToken);

        //return permissions?.Contains(permission) ?? false;
        return false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken) => Task.FromResult<int>(1);
        //_cache.RemoveAsync(_cacheKeys.GetCacheKey(PhoenixClaims.Permission, userId), cancellationToken);
}
