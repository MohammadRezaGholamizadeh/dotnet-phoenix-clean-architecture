namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class UpdatePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
