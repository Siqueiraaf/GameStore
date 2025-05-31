namespace GameStore.PackingService.Features.DTOs;

public record UpdateProductDto(
    string? Name,
    int? Height,
    int? Width,
    int? Length
);
