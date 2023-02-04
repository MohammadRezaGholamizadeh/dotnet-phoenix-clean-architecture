using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.SharedConfiguration.Common.Contracts.Repositories;

namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public interface ApplicationUserRepository : ScopedRepository
    {
        Task<string?> GetUserIdByNationalCode(string nationalCode);
        Task<bool> IsDuplicateNationalCode(string userId, string nationalCode);
        Task<GetApplicationUserDto> GetUserById(string id);
        Task<List<GetApplicationUserDto>> GetAllUsers();
        Task<ApplicationUser?> FindUserByNationalCode(string nationalCode);
        Task<IList<ApplicationUser>> GetAllRegistredUsers(
            string nationalCode,
            string countryCallingCode, 
            string mobileNumber);
        Task<ApplicationUser?> FindUserById(string id);
        Task<bool> isExist(string userId);
        string? GetRoleId(string roleName);
        Task<bool> IsExistByNationalCodeInAllTenants(string nationalCode);
        Task<List<GetUserTenantDto>> GetAllUserTenants(string userName);
    }
}
