using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using YourTale.Application.Contracts.Documents.Responses.User;

namespace WebApplication1.Security;

public class TokenService
{
    public string GenerateToken(UserLoginResponse userLogin)
    {
        var secretKey = Encoding.UTF8.GetBytes(TokenSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(TokenSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", userLogin.Id.ToString()),
                new Claim(ClaimTypes.Email, userLogin.Email),
                new Claim(ClaimTypes.Role, userLogin.Role)
            })
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}