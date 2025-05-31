namespace GameStore.PackingService.Features.DTOs;
public record CreateProductDto(
    string Name,
    int Height,
    int Width,
    int Length
);