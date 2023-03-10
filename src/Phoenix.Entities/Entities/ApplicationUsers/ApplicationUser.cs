using Microsoft.AspNetCore.Identity;
using Phoenix.Domain.Entities.Tenants;
using Phoenix.SharedConfiguration.Multitenancy;

namespace Phoenix.Domain.Entities.ApplicationUsers
{
    public class ApplicationUser : IdentityUser<string>, ITenant
    {
        public ApplicationUser()
        {
            Mobile = new Mobile();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Mobile Mobile { get; set; }
        public string NationalCode { get; set; }
        public DateTime CreationDate { get; set; }
        public string? MobileNumber { get => Mobile.CountryCallingCode + Mobile.MobileNumber; }
        public bool IsActive { get; set; }
        public string TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
