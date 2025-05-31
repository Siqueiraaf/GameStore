using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.Services;
using Microsoft.Extensions.Configuration;

namespace Gamestore.PackingService.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public void GenerateJwtToken_ReturnsValidToken()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Jwt:Key", "v+xLG+9onAmxXxChbLP+6HgFTYx5Lzvw6UXvlO+K7hU="},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"Jwt:ExpireMinutes", "60"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var authService = new AuthService(configuration);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Name = "Test User",
            Password = "dummy-password"
        };

        // Act
        var token = authService.GenerateJwtToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
        // Opcional: validar se o token é um JWT válido
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        Assert.Contains(jwt.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Email && c.Value == user.Email);
    }
}
