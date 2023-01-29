using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.Events;

public interface IEventPublisher : TransientService
{
    Task PublishAsync(IEvent @event);
}
