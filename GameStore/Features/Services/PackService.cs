using GameStore.PackingService.Common.Utils;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services.Interfaces;

namespace GameStore.PackingService.Features.Services;

public class PackService : IPackService
{
    public PackingDto PackOrder(Order order)
    {
        var orderedProducts = order.Products
            .OrderByDescending(p => p.Height * p.Width * p.Length)
            .ToList();

        var response = new PackingDto { OrderId = order.Id };

        foreach (var product in orderedProducts)
        {
            int productVolume = product.Height * product.Width * product.Length;
            bool allocated = false;

            foreach (var usedBox in response.UsedBoxesDto)
            {
                var boxInfo = BoxCatalog.BoxesAvailable.First(b => b.Type == usedBox.BoxType);
                int boxVolume = boxInfo.Height * boxInfo.Width * boxInfo.Length;

                int usedVolume = order.Products
                    .Where(p => usedBox.Products.Contains(p.Id))
                    .Sum(p => p.Height * p.Width * p.Length);

                int availableVolume = boxVolume - usedVolume;

                if (productVolume <= availableVolume)
                {
                    usedBox.Products.Add(product.Id);
                    allocated = true;
                    break;
                }
            }

            if (!allocated)
            {
                var compatibleBox = BoxCatalog.BoxesAvailable
                    .FirstOrDefault(b =>
                        product.Height <= b.Height &&
                        product.Width <= b.Width &&
                        product.Length <= b.Length);

                if (compatibleBox != null)
                {
                    response.UsedBoxesDto.Add(new UsedBoxDto
                    {
                        BoxType = compatibleBox.Type,
                        Products = new List<int> { product.Id }
                    });
                }
            }
        }

        return response;
    }
}
