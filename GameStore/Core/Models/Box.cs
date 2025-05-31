namespace GameStore.PackingService.Core.Models;

public class Box
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }

    // Chave estrangeira
    public int OrderId { get; set; }
    public string? TypeBox { get; internal set; }
}
