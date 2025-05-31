using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services.Interfaces;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;

namespace GameStore.PackingService.Features.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User> RegisterAsync(CreateUserDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser is not null)
            throw new Exception("User already exists.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword
        };

        return await _userRepository.CreateAsync(newUser);
    }

    public async Task<User?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return null;

        return user;
    }

    public async Task<User?> ValidateUserAsync(LoginDto dto)
    {
        // Busca o usuário pelo e-mail
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        // Verifica se o usuário existe e se a senha está correta
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return null;

        // Retorna o usuário se as credenciais estiverem corretas
        return user;
    }
}
