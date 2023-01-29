using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.Caching;

public interface ICacheKeyService : ScopedService
{
    public string GetCacheKey(string name, object id, bool includeTenantId = true);
}
