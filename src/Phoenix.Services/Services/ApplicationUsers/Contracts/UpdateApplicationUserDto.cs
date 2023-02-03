namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class UpdateApplicationUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string CountryCallingCode { get; set; }
        public string NationalCode { get; set; }
        public Guid AvatarId { get; set; }
        public string AvatarExtension { get; set; }
    }
}
