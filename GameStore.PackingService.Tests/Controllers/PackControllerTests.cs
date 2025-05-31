using GameStore.PackingService.Features.Services;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Common.Utils;
using Xunit;
using System.Collections.Generic;

public class PackServiceTests
{
    private readonly PackService _packService = new();

    private readonly List<Box> _originalBoxes;

    public PackServiceTests()
    {
        // Salva o estado original para restaurar depois
        _originalBoxes = BoxCatalog.BoxesAvailable;

        // para testar tem que retirar o readonly
        BoxCatalog.BoxesAvailable =
        [
            new Box { Id = 1, Type = "Caixa 1", Height = 30, Width = 40, Length = 80 },
            new Box { Id = 2, Type = "Caixa 2", Height = 80, Width = 50, Length = 40 },
            new Box { Id = 3, Type = "Caixa 3", Height = 50, Width = 80, Length = 60 },
        ];
    }

    // Restaura o estado original após os testes (pode fazer isso se seu framework suportar Dispose)
    // Se usar xUnit, pode implementar IDisposable para restaurar:
    // public void Dispose() => BoxCatalog.BoxesAvailable = _originalBoxes;

    [Fact]
    public void PackOrder_AllocatesProductToExistingBox_WhenVolumeAllows()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            Products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Height = 5, Width = 5, Length = 5 },
                new() { Id = 2, Name = "Product2", Height = 5, Width = 5, Length = 5 }
            }
        };

        // Act
        var result = _packService.PackOrder(order);

        // Assert
        Assert.Equal(order.Id, result.OrderId);
        Assert.Single(result.UsedBoxesDto);
        Assert.Equal(2, result.UsedBoxesDto[0].Products.Count);
    }

    [Fact]
    public void PackOrder_CreatesNewBox_WhenExistingBoxIsFull()
    {
        // Arrange
        var order = new Order
        {
            Id = 2,
            Products = new List<Product>
        {
            new() { Id = 1, Name = "Big1", Height = 30, Width = 40, Length = 80 }, // volume: 96.000
            new() { Id = 2, Name = "Big2", Height = 30, Width = 40, Length = 80 }  // volume: 96.000
        }
        };

        // Act
        var result = _packService.PackOrder(order);

        // Assert
        Assert.Equal(2, result.UsedBoxesDto.Count); // Agora devem ir em caixas separadas
        Assert.All(result.UsedBoxesDto, box => Assert.Single(box.Products));
    }

    [Fact]
    public void PackOrder_SkipsProduct_WhenNoBoxFits()
    {
        // Arrange
        var order = new Order
        {
            Id = 3,
            Products = new List<Product>
            {
                new() { Id = 1, Name = "Oversize", Height = 100, Width = 100, Length = 100 }
            }
        };

        // Act
        var result = _packService.PackOrder(order);

        // Assert
        Assert.Empty(result.UsedBoxesDto); // Nenhuma caixa suporta o produto
    }
}
