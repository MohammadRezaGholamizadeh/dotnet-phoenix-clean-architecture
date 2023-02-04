namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class GetUserTenantDto
    {
        public string Name { get; set; }
        public string TenantId { get; set; }
        public bool IsTenantBeActive { get; set; }
    }
}
