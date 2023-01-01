using System.Security.Claims;

namespace Phoenix.SharedConfiguration.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.Email);

    public static string? GetTenant(this ClaimsPrincipal principal)
        => principal.FindFirstValue(PhoenixClaims.Tenant);

    public static string? GetFullName(this ClaimsPrincipal principal)
        => principal?.FindFirst(PhoenixClaims.FullName)?.Value;

    public static string? GetFirstName(this ClaimsPrincipal principal)
        => principal?.FindFirst(ClaimTypes.Name)?.Value;

    public static string? GetSurname(this ClaimsPrincipal principal)
        => principal?.FindFirst(ClaimTypes.Surname)?.Value;

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.MobilePhone);

    public static string? GetUserId(this ClaimsPrincipal principal)
       => principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string? GetImageUrl(this ClaimsPrincipal principal)
       => principal.FindFirstValue(PhoenixClaims.ImageUrl);

    public static DateTimeOffset GetExpiration(
        this ClaimsPrincipal principal)
    {
        return DateTimeOffset.FromUnixTimeSeconds(
                Convert.ToInt64(principal
                                .FindFirstValue(PhoenixClaims.Expiration)));
    }

    private static string? FindFirstValue(
        this ClaimsPrincipal principal,
        string claimType)
    {
        return principal is null
               ? throw new ArgumentNullException(nameof(principal))
               : principal.FindFirst(claimType)?.Value;
    }
}