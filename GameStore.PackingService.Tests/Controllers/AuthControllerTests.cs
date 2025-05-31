using Gamestore.PackingService.Features.Controllers;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GameStore.PackingService.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        var dto = new LoginDto("admin@example.com", "123");
        var user = new User { Email = "admin@example.com", Name = "Admin", Password = "123" };

        _userServiceMock.Setup(s => s.ValidateUserAsync(dto)).ReturnsAsync(user);
        _authServiceMock.Setup(s => s.GenerateJwtToken(user)).Returns("fake-token");

        var controller = new AuthController(_userServiceMock.Object, _authServiceMock.Object);

        // Act
        var result = await controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenResponse = Assert.IsType<TokenResponseDto>(okResult.Value);
        Assert.Equal("fake-token", tokenResponse.Token);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserInvalid()
    {
        // Arrange
        var dto = new LoginDto("wrong@example.com", "wrong");
        _userServiceMock.Setup(s => s.ValidateUserAsync(dto)).ReturnsAsync((User?)null);

        var controller = new AuthController(_userServiceMock.Object, _authServiceMock.Object);

        // Act
        var result = await controller.Login(dto);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var dto = new CreateUserDto("fail@example.com", "Fail", "123");

        _userServiceMock.Setup(s => s.RegisterAsync(dto))
            .ThrowsAsync(new Exception("Email already exists"));

        var controller = new AuthController(_userServiceMock.Object, _authServiceMock.Object);

        // Act
        var result = await controller.Register(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Email already exists", badRequest.Value);
    }
}
