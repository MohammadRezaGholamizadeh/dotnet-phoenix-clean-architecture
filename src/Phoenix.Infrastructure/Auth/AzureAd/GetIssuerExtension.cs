
using System.Security.Claims;

namespace Phoenix.Infrastructure.Auth.AzureAd;

public static class GetIssuerExtension
{
    public static string? GetIssuer(this ClaimsPrincipal principal)
    {
        if (principal.FindFirstValue(
            OpenIdConnectClaimTypes.Issuer) is string issuer)
        {
            return issuer;
        }
        return principal.FindFirst(
            AzureADClaimTypes.ObjectId)?.Issuer;
    }
}
