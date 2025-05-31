using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;
using Moq;

namespace Gamestore.PackingService.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    [Fact]
    public async Task RegisterAsync_Throws_WhenUserExists()
    {
        var dto = new CreateUserDto("exist@example.com", "Name", "password");
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(new User
            {
                Id = Guid.NewGuid(),
                Name = "Existing User",
                Email = dto.Email,
                Password = "hashedpassword"
            });

        var service = new UserService(_repositoryMock.Object);

        await Assert.ThrowsAsync<Exception>(() => service.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_ReturnsUser_WhenSuccess()
    {
        var dto = new CreateUserDto("new@example.com", "Name", "password");
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var service = new UserService(_repositoryMock.Object);

        var user = await service.RegisterAsync(dto);

        Assert.Equal(dto.Email, user.Email);
        Assert.NotNull(user.Password);
    }

    [Fact]
    public async Task ValidateUserAsync_ReturnsUser_WhenValid()
    {
        var dto = new LoginDto("test@example.com", "password");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User { Email = dto.Email, Name = "Admin", Password = hashedPassword };

        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        var service = new UserService(_repositoryMock.Object);

        var result = await service.ValidateUserAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task ValidateUserAsync_ReturnsNull_WhenInvalid()
    {
        var dto = new LoginDto("test@example.com", "wrongpass");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpass");
        var user = new User { Email = dto.Email, Name = "Admin", Password = hashedPassword };

        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        var service = new UserService(_repositoryMock.Object);

        var result = await service.ValidateUserAsync(dto);

        Assert.Null(result);
    }
}