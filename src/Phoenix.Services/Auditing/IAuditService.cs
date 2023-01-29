using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Auditing;

public interface IAuditService : TransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}
