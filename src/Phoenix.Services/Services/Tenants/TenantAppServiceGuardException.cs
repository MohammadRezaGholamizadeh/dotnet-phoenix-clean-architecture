using Phoenix.Application.Services.Tenants.Contracts;
using Phoenix.Application.Services.Tenants.Exceptions;
using Phoenix.Domain.Entities.Tenants;
using Phoenix.SharedConfiguration.Attributes.ServiceGuardExceptionAttributes;

namespace Phoenix.Application.Services.Tenants
{
    [ServiceGuardException]
    public partial class TenantAppService : TenantService
    {
        private async Task GuardAgainstDuplicateEmail(
            string email,
            string? id = null)
        {
            if (await _repository.IsDuplicateEmail(email, id))
                throw new DuplicateEmailException();
        }
        private static void GuardAgainstTenantNotExist(Tenant? targetTenant)
        {
            if (targetTenant == null)
                throw new TenantNotExistException();
        }
    }
}
