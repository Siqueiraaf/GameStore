namespace GameStore.PackingService.Features.DTOs;

public record CreateUserDto(
    string Name,
    string Email,
    string Password
);
