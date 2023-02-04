using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.Domain.Entities.Tenants
{
    public class Tenant
    {
        public Tenant()
        {
            Users = new HashSet<ApplicationUser>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public Mobile Mobile { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public HashSet<ApplicationUser> Users { get; set; }
    }
}
