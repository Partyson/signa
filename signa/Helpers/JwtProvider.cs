using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using signa.Entities;
using signa.Interfaces;

namespace signa.Helpers;

public class JwtProvider : IJwtProvider
{

    public string GenerateToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, user.Id.ToString()),
            new (ClaimTypes.Role, user.Role.ToString())
        };
        
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("supermegasigmaultrapupersecretkey"u8.ToArray()),
                SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.AddHours(12)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

