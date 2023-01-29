
using Phoenix.SharedConfiguration.Events;

namespace Phoenix.SharedConfiguration.Common.Contracts;

public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } 
        = DateTime.UtcNow;
}
