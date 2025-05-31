namespace GameStore.PackingService.Features.DTOs;

public class PackingDto
{
    public int OrderId { get; set; }
    public List<UsedBoxDto> UsedBoxesDto { get; set; } = [];
}
