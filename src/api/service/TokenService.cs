using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api.service;
public class TokenService : ITokenService
{
    public string GenerateJwtToken(string username, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]?? "");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, username)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string? GetUsername(HttpContext httpContext)
    {
        var username = httpContext.User.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value
                  ?? httpContext.User.Identity?.Name;

        return username;
    }
}