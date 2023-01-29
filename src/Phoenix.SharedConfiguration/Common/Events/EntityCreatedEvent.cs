using Phoenix.SharedConfiguration.Common.Contracts;

namespace Phoenix.SharedConfiguration.Common.Events;

public static class EntityCreatedEvent
{
    public static EntityCreatedEvent<TEntity>
        WithEntity<TEntity>(TEntity entity) where TEntity : IEntity
    {
        return new(entity);
    }
}

public class EntityCreatedEvent<TEntity> : DomainEvent
    where TEntity : IEntity
{
    internal EntityCreatedEvent(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; }
}
