using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.Services;

namespace Gamestore.PackingService.Tests.Services;

public class PackServiceTests
{
    [Fact]
    public void PackOrder_AllocatesProductsInBoxes()
    {
        // Arrange
        var packService = new PackService();

        var order = new Order
        {
            Id = 1,
            Products =
            [
                new() { Id = 1, Name = "Smartphone", Height = 10, Width = 10, Length = 10 },
                new() { Id = 2, Name = "Headset", Height = 5, Width = 5, Length = 5 },
                new() { Id = 3, Name = "LightBar", Height = 20, Width = 20, Length = 20 }
            ]
        };

        // Act
        var packingDto = packService.PackOrder(order);

        // Assert
        Assert.Equal(order.Id, packingDto.OrderId);
        Assert.NotEmpty(packingDto.UsedBoxesDto);
        Assert.Contains(packingDto.UsedBoxesDto, box => box.Products.Contains(1));
        Assert.Contains(packingDto.UsedBoxesDto, box => box.Products.Contains(2));
        Assert.Contains(packingDto.UsedBoxesDto, box => box.Products.Contains(3));
    }
}
