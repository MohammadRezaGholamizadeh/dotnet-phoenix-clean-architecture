using Ardalis.Specification;
using Phoenix.Application.Common.Persistence;
using Phoenix.Domain.Common.Contracts;
using Phoenix.Domain.Common.Events;

namespace Phoenix.Infrastructure.Persistence.Repository;

/// <summary>
/// The repository that implements IRepositoryWithEvents.
/// Implemented as a decorator. It only augments the Add,
/// Update and Delete calls where it adds the respective
/// EntityCreated, EntityUpdated or EntityDeleted event
/// before delegating to the decorated repository.
/// </summary>
public class EventAddingRepositoryDecorator<T>
    : IRepositoryWithEvents<T>
    where T : class, IAggregateRoot
{
    private readonly IRepository<T> _repository;

    public EventAddingRepositoryDecorator(
        IRepository<T> repository)
    {
        _repository = repository;
    }

    public Task<T> AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityCreatedEvent.WithEntity(entity));
        return _repository.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityUpdatedEvent.WithEntity(entity));
        return _repository.UpdateAsync(entity, cancellationToken);
    }

    public Task DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityDeletedEvent.WithEntity(entity));
        return _repository.DeleteAsync(entity, cancellationToken);
    }

    public Task DeleteRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.DomainEvents.Add(
                EntityDeletedEvent.WithEntity(entity));
        }

        return _repository.DeleteRangeAsync(
                entities, cancellationToken);
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.SaveChangesAsync(cancellationToken);
    }

    public Task<T?> GetByIdAsync<TId>(
        TId id,
        CancellationToken cancellationToken = default) where TId : notnull
    {
        return _repository.GetByIdAsync(id, cancellationToken);
    }

    public Task<T?> GetBySpecAsync<TSpec>(
        TSpec specification,
        CancellationToken cancellationToken = default)
        where TSpec : ISingleResultSpecification, ISpecification<T>
    {
        return _repository.GetBySpecAsync(specification, cancellationToken);
    }

    public Task<TResult?> GetBySpecAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetBySpecAsync(specification, cancellationToken);
    }

    public Task<List<T>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.ListAsync(cancellationToken);
    }

    public Task<List<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return _repository.ListAsync(specification, cancellationToken);
    }

    public Task<List<TResult>> ListAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return _repository.ListAsync(specification, cancellationToken);
    }

    public Task<bool> AnyAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return _repository.AnyAsync(specification, cancellationToken);
    }
    public Task<bool> AnyAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.AnyAsync(cancellationToken);
    }

    public Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return _repository.CountAsync(specification, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _repository.CountAsync(cancellationToken);
    }
}
