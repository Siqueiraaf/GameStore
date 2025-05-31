namespace GameStore.PackingService.Infrastructure.Repositories.Interfaces;

using GameStore.PackingService.Core.Models;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
}
