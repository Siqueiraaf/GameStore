using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;

namespace GameStore.PackingService.Features.Services.Interfaces;

public interface IUserService
{
    Task<User> RegisterAsync(CreateUserDto dto);
    Task<User?> LoginAsync(LoginDto dto);
    Task<User?> ValidateUserAsync(LoginDto dto);
}
