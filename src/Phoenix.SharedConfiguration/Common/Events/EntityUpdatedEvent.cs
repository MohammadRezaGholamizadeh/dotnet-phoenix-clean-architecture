using Phoenix.SharedConfiguration.Common.Contracts;

namespace Phoenix.SharedConfiguration.Common.Events;

public static class EntityUpdatedEvent
{
    public static EntityUpdatedEvent<TEntity> 
        WithEntity<TEntity>(TEntity entity)
        where TEntity : IEntity
    { 
        return new(entity);
    }
}

public class EntityUpdatedEvent<TEntity> : DomainEvent
    where TEntity : IEntity
{
    internal EntityUpdatedEvent(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; }
}
