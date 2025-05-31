namespace GameStore.PackingService.Core.Models;

public class Order
{
    public int Id { get; set; }
    public List<Product> Products { get; set; } = [];
    public List<Box> Boxes { get; set; } = [];
}
