using Microsoft.AspNetCore.Identity;
using Phoenix.Application.Common.Tokens;
using Phoenix.Application.Infrastructures.TokenManagements.Contracts;
using Phoenix.Application.Services.ApplicationUsers.Contracts;
using Phoenix.Application.Services.ApplicationUsers.Exceptions;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.SharedConfiguration.Common.Contracts.UnitOfWorks;
using Phoenix.SharedConfiguration.Tools;
using Phoenix.SharedConfiguration.Validators;
using System.Security.Claims;

namespace Phoenix.Application.Services.ApplicationUsers
{
    public class ApplicationUserAppService : ApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUserRepository _repository;
        private readonly TokenManagementService _tokenManagementService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserTokenService _userTokenService;

        public ApplicationUserAppService(
            UserManager<ApplicationUser> userManager,
            ApplicationUserRepository repository,
            TokenManagementService tokenManager,
            UnitOfWork unitOfWork,
            UserTokenService userTokenService)
        {
            _userManager = userManager;
            _repository = repository;
            _tokenManagementService = tokenManager;
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }
        public async Task<string> AddSuperAdminUserForSeedData(
            ApplicationUser adminUser)
        {
            await GuardAgainstDuplicateUser(string.Empty, adminUser.NationalCode);
            GaurdAgainstInvalidNationalCode(adminUser.NationalCode);
            GaurdAgainstInvalidMobileNumber(adminUser.Mobile.MobileNumber);
            var processResult =
                await _userManager
                      .CreateAsync(
                           adminUser,
                           adminUser.NationalCode);

            GuardAgainstFailedRegisteration(processResult);

            await _unitOfWork.SaveAllChangesAsync();
            return adminUser.Id;
        }
        public async Task<string> AddUser(AddApplicationUserDto dto)
        {
            await GuardAgainstDuplicateUser(string.Empty, dto.NationalCode);
            GaurdAgainstInvalidNationalCode(dto.NationalCode);
            GaurdAgainstInvalidMobileNumber(dto.MobileNumber);

            var applicationUser =
                new Builder<ApplicationUser>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.NationalCode, dto.NationalCode)
                .With(_ => _.UserName, dto.NationalCode)
                .With(_ => _.FirstName, dto.FirstName)
                .With(_ => _.LastName, dto.LastName)
                .With(_ => _.TenantId, _userTokenService.TenantId)
                .With(_ => _.CreationDate, DateTime.UtcNow)
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = dto.CountryCallingCode,
                    MobileNumber = dto.MobileNumber
                })
                .Build();

            var processResult =
                await _userManager
                      .CreateAsync(
                           applicationUser,
                           applicationUser.NationalCode);

            GuardAgainstFailedRegisteration(processResult);

            await _unitOfWork.SaveAllChangesAsync();
            return applicationUser.Id;
        }
        public async Task<string> GenerateTokenForLogin(ApplicationUserLoginDto dto)
        {

            var applicationUser = await _userManager.FindByNameAsync(dto.Username);
            GuardAgainstWrongUserNameOrPassword(applicationUser);
            GuardAgainstInActivateUser(applicationUser.LockoutEnabled);
            var passwordVerified = VerifyHashedPassword(applicationUser, dto.Password);
            GuardAgainstWrongPassword(passwordVerified);

            var userClaims =
                await _userManager.GetClaimsAsync(applicationUser);
            var userRoles =
                await _userManager.GetRolesAsync(applicationUser);

            return _tokenManagementService
                   .Generate(
                        userClaims,
                        userRoles,
                        applicationUser.Id);
        }
        public async Task<IdentityResult> AddUserToAdminRole(string UserId)
        {
            var applicationUser =
                await _userManager.FindByIdAsync(UserId.ToString());
            GuardAgainstUserNotFound(applicationUser);
            var result =
                await _userManager.AddToRoleAsync(
                                   applicationUser,
                                   SystemRoles.Admin);
            GuardAgainstFailedSettingUpUSerToBeAnAdmin(result.Succeeded);
            return result;
        }
        public async Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetRolesAsync(applicationUser);
        }
        public async Task<GetApplicationUserDto> GetUserById(string userId)
        {
            return await _repository.GetUserById(userId);
        }
        public async Task<List<GetApplicationUserDto>> GetAllUsers()
        {
            return await _repository.GetAllUsers();
        }
        public async Task<IdentityResult> DeleteUserFromAdminRole(string id)
        {
            var applicationUser =
                await _userManager.FindByIdAsync(id.ToString());
            GuardAgainstUserNotFound(applicationUser);
            var result =
                await _userManager.RemoveFromRoleAsync(applicationUser, SystemRoles.Admin);
            GuardAgainstRemoveUserFromAdminRoleFailed(result.Succeeded);
            return result;
        }
        public async Task<ApplicationUser> FindByUsername(string username)
        {
            var applicationUser =
                await _userManager.FindByNameAsync(username);
            GuardAgainstUserNotFound(applicationUser);
            return applicationUser;
        }
        public bool VerifyHashedPassword(
            ApplicationUser applicationUser,
            string dtoPassword)
        {
            var passwordVerifiedResult
                = _userManager.PasswordHasher
                              .VerifyHashedPassword(applicationUser,
                                                    applicationUser.PasswordHash,
                                                    dtoPassword);

            GuardAgainstWrongPassword(passwordVerifiedResult
                                      .Equals(PasswordVerificationResult.Success));
            return true;
        }
        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetClaimsAsync(applicationUser);
        }
        public async Task UpdatePassword(string userId, UpdatePasswordDto dto)
        {
            var user =
                await _userManager.FindByIdAsync(userId.ToString());
            GuardAgainstUserNotFound(user);
            var checkResult =
                await _userManager.CheckPasswordAsync(user, dto.OldPassword);
            GuardAginstInCorrectCurrentPassword(checkResult);
            var changeResult =
                await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            GuardAgainstUpdatePasswordFailed(changeResult);
        }
        public async Task UpdateUser(string userId, UpdateApplicationUserDto dto)
        {
            var applicationUser =
                await _userManager.FindByIdAsync(userId.ToString());
            await GuardAgainstDuplicateUser(userId, dto.NationalCode);
            GaurdAgainstInvalidNationalCode(dto.NationalCode);
            GaurdAgainstInvalidMobileNumber(dto.MobileNumber);
            applicationUser.FirstName = dto.FirstName;
            applicationUser.LastName = dto.LastName;
            applicationUser.NationalCode = dto.NationalCode;

            await _userManager.UpdateAsync(applicationUser);
        }
        public async Task<ApplicationUser> ValidateUser(string username, string password)
        {
            var applicationUser =
                await FindByUsername(username);
            var passwordVerified =
                VerifyHashedPassword(applicationUser, password);
            GuardAgainstWrongUserNameOrPassword(applicationUser);
            GuardAgainstInActivateUser(applicationUser.LockoutEnabled);
            return passwordVerified ? applicationUser : null;
        }
        public Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
        private static void GuardAgainstInActivateUser(bool lockoutEnabled)
        {
            if (lockoutEnabled)
                throw new UserIsInactiveException();
        }
        private void GuardAgainstWrongUserNameOrPassword(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                throw new WrongUsernameOrPasswordException();
        }
        private void GuardAgainstWrongPassword(bool passwordVerified)
        {
            if (!passwordVerified)
                throw new WrongUsernameOrPasswordException();
        }
        private static void GuardAgainstFailedRegisteration(IdentityResult processResult)
        {
            if (!processResult.Succeeded)
                throw new CreateUserFailedException();
        }
        private void GuardAgainstUserNotFound(ApplicationUser? user)
        {
            if (user == null)
                throw new UserNotFoundException();
        }
        private async Task GuardAgainstDuplicateUser(string userId, string nationalCode)
        {
            if (await _repository.IsDuplicateNationalCode(userId, nationalCode))
                throw new NationalCodeAlreadyRegisteredException();
        }
        private void GaurdAgainstInvalidNationalCode(string nationalCode)
        {
            if (NationalCodeValidator.IsValid(nationalCode) == false)
                throw new InvalidNationalCodeException();
        }
        private void GaurdAgainstInvalidMobileNumber(string mobileNumber)
        {
            if (new MobileNumberAttribute().IsValid(mobileNumber) == false)
                throw new InvalidMobileNumberException();
        }
        private static void GuardAgainstFailedSettingUpUSerToBeAnAdmin(bool succeeded)
        {
            if (!succeeded)
                throw new SettingUpUserToAdminRoleFailedException();
        }
        private static void GuardAgainstRemoveUserFromAdminRoleFailed(bool succeeded)
        {
            if (!succeeded)
                throw new RemoveUserFromAdminRoleFailedException();
        }
        private static void GuardAgainstUpdatePasswordFailed(IdentityResult changeResult)
        {
            if (!changeResult.Succeeded)
                throw new ChangePasswordFailedException();
        }
        private static void GuardAginstInCorrectCurrentPassword(bool checkResult)
        {
            if (!checkResult)
                throw new CurrentPasswordIsNotCorrectException();
        }

        public async Task<bool> IsExistNationalCode(string nationalCode)
        {
            return await _repository.IsExistByNationalCodeInAllTenants(nationalCode);
        }

        public async Task<List<GetUserTenantDto>> GetAllUserTenants(string userName)
        {
            return await _repository.GetAllUserTenants(userName);
        }
    }
}

