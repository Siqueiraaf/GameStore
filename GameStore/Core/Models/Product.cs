namespace GameStore.PackingService.Core.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }

    // Chave estrangeira referente ao pedido
    public int OrderId { get; set; }
}
