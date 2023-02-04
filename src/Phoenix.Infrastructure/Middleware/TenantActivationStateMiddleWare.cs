using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.Application.Common.Tokens;
using Phoenix.Application.Services.Tenants.Contracts;
using Phoenix.Application.Services.Tenants.Exceptions;
using Phoenix.Infrastructure.Middleware.Attributes;
using System.Reflection;

namespace Phoenix.Infrastructure.Middleware
{
    public class TenantActivationStateMiddleWare
        : IFilterMetadata
    {
        private readonly TenantService _tenantService;
        private readonly UserTokenService _userTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantActivationStateMiddleWare(
            TenantService tenantService,
            UserTokenService userTokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _tenantService = tenantService;
            _userTokenService = userTokenService;
            _httpContextAccessor = httpContextAccessor;
            Invoke();
        }

        public void Invoke()
        {
            var httpContextEndPoint =
                _httpContextAccessor.HttpContext?.GetEndpoint();
            var request = httpContextEndPoint?.RequestDelegate?.Target;
            var action = request?.GetType()
                          ?.GetRuntimeField("actionDescriptor")
                          ?.GetValue(request) as ActionDescriptor;

            var hasJumpOverMiddleWare =
                 action?.EndpointMetadata
                       .SingleOrDefault(_ =>
                         _.GetType()
                           == typeof(JumpOverMiddleWareAttribute)) != null;

            var isActive =
                Task.Run(async () =>
                         await _tenantService
                               .IsActiveById(
                                 _userTokenService.TenantId)).Result;

            if (!hasJumpOverMiddleWare && !isActive)
                throw new InActivatedTenantException();
        }
    }
}
