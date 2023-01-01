namespace Phoenix.Infrastructure.Auth;

public class SecurityConfiguration
{
    public string? Provider { get; set; }
    public bool RequireConfirmedAccount { get; set; }
}
