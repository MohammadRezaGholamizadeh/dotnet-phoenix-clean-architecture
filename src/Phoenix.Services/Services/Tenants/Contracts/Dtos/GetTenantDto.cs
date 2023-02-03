namespace Phoenix.Application.Services.Tenants.Contracts.Dtos
{
    public class GetTenantDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryCallingCode { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
