using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Phoenix.Application.Infrastructures.TokenManagements.Contracts;
using Phoenix.SharedConfiguration.Tools;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Phoenix.Application.Infrastructures.TokenManagements
{
    public class TokenManagementAppService : TokenManagementService
    {
        private readonly JwtBearerTokenConfig _jwtBearerTokenSettings;

        public TokenManagementAppService(IOptions<JwtBearerTokenConfig> jwtTokenOptions)
        {
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
        }

        public string Generate(
            IList<Claim> userClaims,
            IList<string> userRoles,
            string applicationUserId)
        {
            var jwtSecurityTokenHandler =
                new JwtSecurityTokenHandler();

            var secretKey =
                Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(
                new Claim(ClaimTypes.NameIdentifier,
                          applicationUserId));

            AddUserRolesToTokenClaims(ref claimsIdentity, userRoles);
            AddUserClaimsToTokenClaims(ref claimsIdentity, userClaims);

            var securityTokenDescriptor =
                new Builder<SecurityTokenDescriptor>()
                .With(_ => _.Subject, claimsIdentity)
                .With(_ => _.Expires, DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds))
                .With(_ => _.SigningCredentials, new SigningCredentials(new SymmetricSecurityKey(secretKey),
                                                                        SecurityAlgorithms.HmacSha256Signature))
                .With(_ => _.Audience, _jwtBearerTokenSettings.Audience)
                .With(_ => _.Issuer, _jwtBearerTokenSettings.Issuer)
                .Build();

            var token = jwtSecurityTokenHandler
                        .CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        private void AddUserClaimsToTokenClaims(
            ref ClaimsIdentity claimsIdentity,
            IList<Claim> userClaims)
        {
            foreach (var claim in userClaims)
            {
                claimsIdentity.AddClaim(new Claim(claim.Type, claim.Value));
            }
        }

        private void AddUserRolesToTokenClaims(
            ref ClaimsIdentity claimsIdentity,
            IList<string> userRoles)
        {
            foreach (var role in userRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
    }
}
