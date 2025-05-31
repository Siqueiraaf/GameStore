namespace GameStore.PackingService.Features.DTOs;

public class UsedBoxDto
{
    public required string BoxType { get; set; }
    public List<int> Products { get; set; } = [];
}
