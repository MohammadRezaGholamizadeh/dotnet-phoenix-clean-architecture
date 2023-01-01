using Phoenix.Application.Common.Interfaces;

namespace Phoenix.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}
