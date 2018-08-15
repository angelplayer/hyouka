using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hyouka_api.Infrastructure.security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public readonly JwtIssuerOptions option;

        public JwtTokenGenerator(JwtIssuerOptions option)
        {
            this.option = option;
        }

        public async Task<string> CreateToke(string username)
        {
            var claims = new[]
             {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await option.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                  new DateTimeOffset(option.IssuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)

            };
            var jwt = new JwtSecurityToken(
                option.Issuer,
                option.Audience,
                claims,
                option.NotBefore,
                option.Expiration,
                option.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}