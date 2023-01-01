using Phoenix.Application.Common.Interfaces;
namespace Phoenix.Application.Common.Caching;

public interface ICacheKeyService : IScopedService
{
    public string GetCacheKey(string name, object id, bool includeTenantId = true);
}
