using Phoenix.SharedConfiguration.Common.Contracts.Services;
using System.Security.Claims;

namespace Phoenix.Application.Infrastructures.TokenManagements.Contracts
{
    public interface TokenManagementService : ScopedService
    {
        string Generate(
            IList<Claim> userClaims,
            IList<string> userRoles,
            string applicationUserId);
    }
}
