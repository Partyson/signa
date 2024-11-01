using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using signa.Entities;
using signa.Interfaces;

namespace signa.Helpers;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions options = options.Value;

    public string GenerateToken(UserEntity user)
    {
        var token = new JwtSecurityToken(
            claims: [new Claim("userId", user.Id.ToString())],
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("supermegasigmaultrapupersecretkey"u8.ToArray()),
                SecurityAlgorithms.HmacSha256),
            
            expires: DateTime.UtcNow.AddHours(12)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

