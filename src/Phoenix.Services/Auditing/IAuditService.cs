using Phoenix.Application.Common.Interfaces;
namespace Phoenix.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}
