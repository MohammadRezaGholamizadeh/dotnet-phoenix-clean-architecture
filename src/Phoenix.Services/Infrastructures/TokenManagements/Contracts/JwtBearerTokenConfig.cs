namespace Phoenix.Application.Infrastructures.TokenManagements.Contracts
{
    public class JwtBearerTokenConfig
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryTimeInSeconds { get; set; }
    }
}
