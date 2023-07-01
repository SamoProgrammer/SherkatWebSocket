using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SherkatWebSocketApi.Authentication.Entities;

namespace SherkatWebSocketApi.Authentication;

public class JWTManagerRepository : IJWTManagerRepository
{
    private readonly IConfiguration _configuration;
    public JWTManagerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public JWTToken Authenticate(User user)
    {
        // Else we generate JSON Web Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JWTToken { Token = tokenHandler.WriteToken(token), Expires = token.ValidTo };
    }
}