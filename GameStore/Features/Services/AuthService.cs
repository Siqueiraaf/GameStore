using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GameStore.PackingService.Features.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJwtToken(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");

        var key = jwtSection["Key"] ?? throw new Exception("JWT Key is missing in configuration.");
        var issuer = jwtSection["Issuer"] ?? throw new Exception("JWT Issuer is missing.");
        var audience = jwtSection["Audience"] ?? throw new Exception("JWT Audience is missing.");
        var expireMinutes = int.TryParse(jwtSection["ExpireMinutes"], out var minutes)
            ? minutes
            : throw new Exception("Invalid JWT expiration time.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
