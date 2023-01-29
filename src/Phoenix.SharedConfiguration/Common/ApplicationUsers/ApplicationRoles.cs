using Microsoft.AspNetCore.Identity;

namespace Phoenix.SharedConfiguration.Common.ApplicationUsers
{
    public class ApplicationRoles : IdentityRole
    {
        public string? Description { get; set; }

        public ApplicationRoles(string name, string? description = null)
            : base(name)
        {
            Description = description;
            NormalizedName = name.ToUpperInvariant();
        }
    }
}