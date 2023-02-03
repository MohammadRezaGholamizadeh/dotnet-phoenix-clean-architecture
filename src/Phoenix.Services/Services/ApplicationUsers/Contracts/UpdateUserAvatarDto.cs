namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class UpdateUserAvatarDto
	{
		public Guid AvatarId { get; set; }
		public string AvatarExtension { get; set; }
	}
}
