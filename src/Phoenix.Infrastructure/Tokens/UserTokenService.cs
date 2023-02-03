using Microsoft.AspNetCore.Http;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.SharedConfiguration.Common.Contracts.Services;
using System.Security.Claims;

namespace Phoenix.Infrastructure.Tokens
{
    public interface UserTokenService : ScopedService
    {
        string? UserId { get; }
        IList<string> Roles { get; }
        bool Admin { get; }
        bool Guest { get; }

    }


    public class UserTokenAppService : UserTokenService
    {
        private readonly IHttpContextAccessor _accessor;

        public UserTokenAppService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string? UserId => GetUserIdFromJwtToken();

        public IList<string> Roles
        {
            get
            {
                return _accessor.HttpContext.User.Claims.Where(_ => _.Type == ClaimTypes.Role)
                     .Select(_ => _.Value)
                     .ToList();
            }
        }

        public bool Admin => Roles.Any(_ => _ == SystemRoles.Admin);
        public bool Guest => Roles.Any(_ => _ == SystemRoles.Guest);


        private string? GetUserIdFromJwtToken()
        {
            return _accessor.HttpContext?
                   .User.Claims
                   .FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
