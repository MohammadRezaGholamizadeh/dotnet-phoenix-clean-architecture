using Phoenix.Application.Services.Tenants.Contracts;
using Phoenix.Application.Services.Tenants.Contracts.Dtos;

namespace Phoenix.RestApi.Controllers.Tenants
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tenants")]
    [ApiVersion("1.0")]
    public class TenantsController : ControllerBase
    {
        private readonly TenantService _tenantService;

        public TenantsController(TenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost(Name = "AddTenant")]
        public async Task<string> AddTenant(AddTenantDto dto)
        {
            return await _tenantService.Add(dto);
        }

        [HttpPut("{id}", Name = "UpdateTenant")]
        public async Task Update(string id, UpdateTenantDto dto)
        {
            await _tenantService.Update(id, dto);
        }

        [HttpDelete("{id}", Name = "DeleteTenant")]
        public async Task Delete(string id)
        {
            await _tenantService.DeleteById(id);
        }

        [HttpGet("{id}", Name = "GetTenantById")]
        public async Task<GetTenantDto?> GetById(string id)
        {
            return await _tenantService.GetById(id);
        }
    }
}