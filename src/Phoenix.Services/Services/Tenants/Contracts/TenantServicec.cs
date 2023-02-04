using Phoenix.Application.Services.Tenants.Contracts.Dtos;
using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Services.Tenants.Contracts
{
    public interface TenantService : ScopedService
    {
        Task<string> Add(AddTenantDto dto);
        Task Update(string id, UpdateTenantDto dto);
        Task DeleteById(string id);
        Task<GetTenantDto?> GetById(string id);
        Task<List<GetTenantDto>> GetAll();
        Task<bool> IsActiveById(string tenantId);
        Task ToggleActivationStatus(string id);
    }
}
