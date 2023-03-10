using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Phoenix.SharedConfiguration.Common.Contracts.Services;
using static Phoenix.Infrastructure.Notifications.NotificationHub;

namespace Phoenix.Infrastructure.Notifications;

[Authorize]
public class NotificationHub : Hub, NotificationHubService
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        //if (_currentTenant is null)
        //{
        //    throw new UnauthorizedException("Authentication Failed.");
        //}

        await Groups.AddToGroupAsync(Context.ConnectionId, $"GroupTenant-");

        await base.OnConnectedAsync();

        _logger.LogInformation("A client connected to NotificationHub: {connectionId}", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"GroupTenant-");

        await base.OnDisconnectedAsync(exception);

        _logger.LogInformation("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
    }
    public interface NotificationHubService : TransientService
    {

    }
}
