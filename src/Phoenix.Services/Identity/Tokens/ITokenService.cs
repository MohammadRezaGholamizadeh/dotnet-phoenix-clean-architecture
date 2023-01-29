using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Identity.Tokens;

public interface ITokenService : TransientService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}
