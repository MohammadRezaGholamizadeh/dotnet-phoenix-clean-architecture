using Microsoft.AspNetCore.Authorization;
using Phoenix.Application.Common.Tokens;
using Phoenix.Application.Infrastructures.TokenManagements.Contracts;
using Phoenix.Application.Services.ApplicationUsers.Contracts;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.Infrastructure.Middleware.Attributes;

namespace Phoenix.RestApi.Controllers.ApplicationUsers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/user-management")]
    [ApiVersion("1.0")]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationUserService _service;
        private readonly UserTokenService _userTokenService;
        private readonly TokenManagementService _tokenService;

        public ApplicationUsersController(
            ApplicationUserService service,
            UserTokenService userTokenService,
            TokenManagementService tokenService)
        {
            _service = service;
            _userTokenService = userTokenService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [JumpOverMiddleWare]
        public async Task<string> LogIn(ApplicationUserLoginDto dto)
        {
            var applicationUser =
                await _service.ValidateUser(dto.Username, dto.Password);

            return _tokenService
                   .Generate(
                        await _service.GetClaimsAsync(applicationUser!),
                        await _service.GetRolesAsync(applicationUser!),
                        applicationUser!.Id);
        }

        [HttpPost("register-user")]
        [Authorize(Roles = SystemRoles.Admin)]
        public async Task AddUser(AddApplicationUserDto dto)
        {
            await _service.AddUser(dto);
        }

        [HttpGet]
        public async Task<List<GetApplicationUserDto>> GetAllUsers()
        {
            return await _service.GetAllUsers();
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = SystemRoles.Admin)]
        public async Task UpdateUser(
            string id,
            UpdateApplicationUserDto dto)
        {
            await _service.UpdateUser(id, dto);
        }

        [HttpPost("update-password")]
        [Authorize(Roles = SystemRoles.Admin)]
        public async Task UpdatePassword(UpdatePasswordDto dto)
        {
            await _service.UpdatePassword(_userTokenService.UserId, dto);
        }

        [HttpPatch("{userId}/set-admin")]
        [Authorize(Roles = SystemRoles.Admin)]
        public async Task SetUserToRole(string userId)
        {
            await _service.AddUserToAdminRole(userId);
        }

        [HttpPatch("{userId}/unset-admin")]
        [Authorize(Roles = SystemRoles.Admin)]
        public async Task RemoveUserFromRole(string userId)
        {
            await _service.DeleteUserFromAdminRole(userId);
        }

        [HttpGet("user-profile")]
        [Authorize]
        public async Task<GetApplicationUserDto> GetUserProfile()
        {
            return await _service.GetUserById(_userTokenService.UserId);
        }
    }
}