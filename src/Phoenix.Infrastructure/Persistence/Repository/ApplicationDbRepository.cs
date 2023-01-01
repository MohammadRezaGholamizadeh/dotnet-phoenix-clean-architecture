using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using Phoenix.Application.Common.Persistence;
using Phoenix.Domain.Common.Contracts;
using Phoenix.Infrastructure.Multitenancy;

namespace Phoenix.Infrastructure.Persistence.Repository;

public class ApplicationDbRepository<T> 
    : RepositoryBase<T>, 
      IReadRepository<T>,
      IRepository<T>
    where T : class, IAggregateRoot
{
    public ApplicationDbRepository(EFDataContext dbContext)
        : base(dbContext)
    {
    }

    protected override IQueryable<TResult> ApplySpecification<TResult>(
        ISpecification<T, TResult> specification)
    {
        return specification.Selector is not null
               ? base.ApplySpecification(specification)
               : ApplySpecification(specification, false)
                 .ProjectToType<TResult>(); ;
    }

}
