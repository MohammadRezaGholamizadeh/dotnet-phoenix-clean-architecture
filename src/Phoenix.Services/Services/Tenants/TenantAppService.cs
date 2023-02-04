using Phoenix.Application.Services.Tenants.Contracts;
using Phoenix.Application.Services.Tenants.Contracts.Dtos;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.Domain.Entities.Tenants;
using Phoenix.SharedConfiguration.Common.Contracts.UnitOfWorks;
using Phoenix.SharedConfiguration.Tools;

namespace Phoenix.Application.Services.Tenants
{
    public partial class TenantAppService : TenantService
    {
        private readonly TenantRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public TenantAppService(
            TenantRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Add(AddTenantDto dto)
        {
            await GuardAgainstDuplicateEmail(dto.Email);
            var tenant =
                new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, dto.Name)
                .With(_ => _.IsActive, true)
                .With(_ => _.Email, dto.Email)
                .With(_ => _.Mobile, new Mobile()
                {
                    MobileNumber = dto.MobileNumber.TrimStart('0'),
                    CountryCallingCode = dto.CountryCallingCode
                })
                .Build();

            _repository.Add(tenant);

            await _unitOfWork.SaveAllChangesAsync();
            return tenant.Id;
        }

        public async Task DeleteById(string id)
        {
            var targetTenant =
                await _repository.FindWithAllUsers(id);
            GuardAgainstTenantNotExist(targetTenant);

            _repository.Delete(targetTenant);

            await _unitOfWork.SaveAllChangesAsync();
        }

        public async Task<List<GetTenantDto>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<GetTenantDto?> GetById(string id)
        {
            return await _repository.GetById(id);
        }

        public async Task<bool> IsActiveById(string tenantId)
        {
            return await _repository.IsActiveById(tenantId);
        }

        public async Task ToggleActivationStatus(string id)
        {
            var targetTenant =
                await _repository.FindById(id);
            GuardAgainstTenantNotExist(targetTenant);
            targetTenant!.IsActive = !targetTenant.IsActive;

            await _unitOfWork.SaveAllChangesAsync();
        }

        public async Task Update(string id, UpdateTenantDto dto)
        {
            var targetTenant =
                await _repository.FindById(id);
            GuardAgainstTenantNotExist(targetTenant);
            await GuardAgainstDuplicateEmail(dto.Email, id);

            targetTenant!.Name = dto.Name;
            targetTenant.Email = dto.Email;
            targetTenant.Mobile = new Mobile()
            {
                CountryCallingCode = dto.CountryCallingCode,
                MobileNumber = dto.MobileNumber
            };

            await _unitOfWork.SaveAllChangesAsync();
        }

    }
}
