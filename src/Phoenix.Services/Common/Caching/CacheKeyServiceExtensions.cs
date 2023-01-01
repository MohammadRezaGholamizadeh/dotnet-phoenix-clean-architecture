namespace Phoenix.Application.Common.Caching;
using Phoenix.Domain.Common.Contracts;

public static class CacheKeyServiceExtensions
{
    public static string GetCacheKey<TEntity>(this ICacheKeyService cacheKeyService, object id, bool includeTenantId = true)
    where TEntity : IEntity =>
        cacheKeyService.GetCacheKey(typeof(TEntity).Name, id, includeTenantId);
}
