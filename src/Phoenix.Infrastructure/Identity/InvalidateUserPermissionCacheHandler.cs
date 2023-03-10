using Microsoft.AspNetCore.Identity;
using Phoenix.Application.Common.Events;
using Phoenix.Application.Services.ApplicationUsers.Contracts;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.SharedConfiguration.Identity;

namespace Phoenix.Infrastructure.Identity;

internal class InvalidateUserPermissionCacheHandler :
    IEventNotificationHandler<ApplicationUserUpdatedEvent>,
    IEventNotificationHandler<ApplicationRoleUpdatedEvent>
{
    private readonly ApplicationUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public InvalidateUserPermissionCacheHandler(ApplicationUserService userService, UserManager<ApplicationUser> userManager) =>
        (_userService, _userManager) = (userService, userManager);

    public async Task Handle(EventNotification<ApplicationUserUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        //if (notification.Event.RolesUpdated)
        //{
        //    await _userService.InvalidatePermissionCacheAsync(notification.Event.UserId, cancellationToken);
        //}
    }

    public async Task Handle(EventNotification<ApplicationRoleUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        //if (notification.Event.PermissionsUpdated)
        //{
        //    foreach (var user in await _userManager.GetUsersInRoleAsync(notification.Event.RoleName))
        //    {
        //        await _userService.InvalidatePermissionCacheAsync(user.Id, cancellationToken);
        //    }
        //}
    }
}
