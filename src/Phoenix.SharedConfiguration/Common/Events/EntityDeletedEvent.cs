using Phoenix.SharedConfiguration.Common.Contracts;

namespace Phoenix.SharedConfiguration.Common.Events;

public static class EntityDeletedEvent
{
    public static EntityDeletedEvent<TEntity> 
        WithEntity<TEntity>(TEntity entity)
        where TEntity : IEntity
    {
        return new(entity);
    }

}

public class EntityDeletedEvent<TEntity> : DomainEvent
    where TEntity : IEntity
{
    internal EntityDeletedEvent(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; }
}
