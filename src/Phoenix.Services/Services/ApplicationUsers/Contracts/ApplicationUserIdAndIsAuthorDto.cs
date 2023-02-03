namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class ApplicationUserIdAndIsAuthorDto
    {
        public Guid UserId { get; set; }
        public bool IsAuthor { get; set; }
    }
}
