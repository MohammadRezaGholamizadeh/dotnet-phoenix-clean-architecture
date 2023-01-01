namespace Phoenix.Infrastructure.SecurityHeaders;

public class SecurityHeaderConfiguration
{
    public bool Enable { get; set; }
    public string? X_FrameOptions { get; set; }
    public string? X_Content_Type_Options { get; set; }
    public string? Referrer_Policy { get; set; }
    public string? Permissions_Policy { get; set; }
    public string? SameSite { get; set; }
    public string? X_XSS_Protection { get; set; }
}
