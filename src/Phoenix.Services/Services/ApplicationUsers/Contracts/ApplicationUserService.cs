using Microsoft.AspNetCore.Identity;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.SharedConfiguration.Common.Contracts.Services;
using System.Security.Claims;
namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public interface ApplicationUserService : ScopedService 
    {
        Task<string> AddUser(AddApplicationUserDto dto);
        Task<ApplicationUser> FindByUsername(string username);
        Task<GetApplicationUserDto> GetUserById(string id);
        Task<List<GetApplicationUserDto>> GetAllUsers();
        bool VerifyHashedPassword(ApplicationUser applicationUser, string dtoPassword);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser);
        Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser);
        Task<string> GenerateTokenForLogin(ApplicationUserLoginDto dto);
        Task UpdateUser(string userId, UpdateApplicationUserDto appUserUpdateDto);
        Task UpdatePassword(string id, UpdatePasswordDto dto);
        Task<IdentityResult> AddUserToAdminRole(string UserId);
        Task<bool> IsExistNationalCcode(string nationalCode);
        Task<IdentityResult> DeleteUserFromAdminRole(string UserId);
        Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
        Task<ApplicationUser> ValidateUser(string username, string password);
    }
}
