using Microsoft.AspNetCore.Identity;

namespace Phoenix.SharedConfiguration.Common.ApplicationUsers
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string? CreatedBy { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}