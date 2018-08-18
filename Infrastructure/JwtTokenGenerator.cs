using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace hyouka_api.Infrastructure.security
{
  public class JwtTokenGenerator : IJwtTokenGenerator
  {
    public readonly JwtIssuerOptions jwtOption;

    public JwtTokenGenerator(IOptions<JwtIssuerOptions> option)
    {
      this.jwtOption = option.Value;
    }

    public async Task<string> CreateToke(string username)
    {
      var claims = new[]
       {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOption.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                  new DateTimeOffset(jwtOption.IssuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)

            };
      var jwt = new JwtSecurityToken(
          jwtOption.Issuer,
          jwtOption.Audience,
          claims,
          jwtOption.NotBefore,
          jwtOption.Expiration,
          jwtOption.SigningCredentials);
      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }
  }
}