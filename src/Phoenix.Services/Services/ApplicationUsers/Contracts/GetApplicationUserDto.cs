namespace Phoenix.Application.Services.ApplicationUsers.Contracts
{
    public class GetApplicationUserDto 
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string CountryCallingCode { get; set; }
        public string AvatarId { get; set; }
        public string AvatarExtension { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
