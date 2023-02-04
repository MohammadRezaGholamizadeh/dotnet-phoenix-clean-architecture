using Phoenix.SharedConfiguration.Validators;
using System.ComponentModel.DataAnnotations;

namespace Phoenix.Application.Services.Tenants.Contracts.Dtos
{
    public class AddTenantDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(5)]
        public string CountryCallingCode { get; set; }
        [Required]
        [MobileNumber]
        [MaxLength(10)]
        public string MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
