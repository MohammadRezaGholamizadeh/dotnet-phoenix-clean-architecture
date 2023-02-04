using Phoenix.Application.Services.Tenants.Contracts.Dtos;
using Phoenix.Domain.Entities.Tenants;
using Phoenix.SharedConfiguration.Common.Contracts.Repositories;

namespace Phoenix.Application.Services.Tenants.Contracts
{
    public interface TenantRepository : ScopedRepository
    {
        void Add(Tenant tenant);
        Task<bool> IsDuplicateEmail(string email, string? id = null);
        Task<Tenant?> FindById(string id);
        Task<Tenant?> FindWithAllUsers(string id);
        void Delete(Tenant targetTenant);
        Task<GetTenantDto?> GetById(string id);
        Task<List<GetTenantDto>> GetAll();
        Task<bool> IsActiveById(string tenantId);
    }
}
