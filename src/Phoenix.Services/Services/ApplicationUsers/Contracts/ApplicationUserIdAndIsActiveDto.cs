namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class ApplicationUserIdAndIsActiveDto
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
