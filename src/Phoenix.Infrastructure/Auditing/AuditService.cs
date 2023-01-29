using Mapster;
using Microsoft.EntityFrameworkCore;
using Phoenix.Application.Auditing;
using Phoenix.DataSources.Infrastructures.DBContexts;

namespace Phoenix.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly EFDataContext _context;

    public AuditService(EFDataContext context) => _context = context;

    public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
    {
        var trails = await _context.Set<Trail>()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }
}
