using Microsoft.EntityFrameworkCore;
using Phoenix.Application.Services.Tenants.Contracts;
using Phoenix.Application.Services.Tenants.Contracts.Dtos;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Domain.Entities.Tenants;

namespace Phoenix.Persistance.EF.Repositories.Tenants
{
    public class EFTenantRepository : TenantRepository
    {
        private DbSet<Tenant> _tenants;

        public EFTenantRepository(EFDataContext context)
        {
            _tenants = context.Set<Tenant>();
        }

        public void Add(Tenant tenant)
        {
            _tenants.Add(tenant);
        }

        public void Delete(Tenant targetTenant)
        {
            _tenants.Remove(targetTenant);
        }

        public async Task<Tenant?> FindById(string id)
        {
            return await _tenants.FindAsync(id);
        }

        public async Task<Tenant?> FindWithAllUsers(string id)
        {
            return await _tenants
                         .Include(_ => _.Users)
                         .SingleOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<List<GetTenantDto>> GetAll()
        {
            return await _tenants
                         .Select(_ => new GetTenantDto()
                         {
                             Id = _.Id,
                             Name = _.Name,
                             MobileNumber = _.Mobile.MobileNumber,
                             CountryCallingCode = _.Mobile.CountryCallingCode,
                             Email = _.Email,
                             IsActive = _.IsActive
                         })
                         .ToListAsync();
        }

        public async Task<GetTenantDto?> GetById(string id)
        {
            return await
                _tenants
                .Where(_ => _.Id == id)
                .Select(_ => new GetTenantDto()
                {
                    Id = _.Id,
                    Name = _.Name,
                    MobileNumber = _.Mobile.MobileNumber,
                    CountryCallingCode = _.Mobile.CountryCallingCode,
                    Email = _.Email,
                    IsActive = _.IsActive
                }).SingleOrDefaultAsync();
        }

        public async Task<bool> IsActiveById(string tenantId)
        {
            return await _tenants.AnyAsync(_ => _.Id == tenantId && _.IsActive);
        }

        public async Task<bool> IsDuplicateEmail(
            string email,
            string? id = null)
        {
            email = email.ToLower();
            return await _tenants
                         .AnyAsync(_ => _.Email!.ToLower()
                                     == email && _.Id != id);
        }
    }
}
