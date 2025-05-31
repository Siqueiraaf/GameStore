using GameStore.PackingService.Core.Models;

namespace GameStore.PackingService.Infrastructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetProductsByOrderIdAsync(int orderId);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);

    Task DeleteProductsByOrderIdAsync(int orderId);
}
