using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;

namespace GameStore.PackingService.Features.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByOrderIdAsync(int orderId);
    Task AddAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(int productId, UpdateProductDto dto);
    Task DeleteByOrderIdAsync(int orderId);
}

