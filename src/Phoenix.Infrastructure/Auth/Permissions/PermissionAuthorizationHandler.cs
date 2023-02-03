using Microsoft.AspNetCore.Authorization;
using Phoenix.Application.Services.ApplicationUsers.Contracts;

namespace Phoenix.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler 
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly ApplicationUserService _userService;

    public PermissionAuthorizationHandler(
        ApplicationUserService userService)
    {
        _userService = userService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        //if (context.User?.GetUserId() is { } userId &&
        //    await _userService.HasPermissionAsync(
        //           userId, 
        //           requirement.Permission))
        //{
        //    context.Succeed(requirement);
        //}
        context.Succeed(requirement);
    }
}
