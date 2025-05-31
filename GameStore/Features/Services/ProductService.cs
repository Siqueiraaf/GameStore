using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services.Interfaces;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;

namespace GameStore.PackingService.Features.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetProductsAsync();
    }

    public async Task<IEnumerable<Product>> GetByOrderIdAsync(int orderId)
    {
        return await _repository.GetProductsByOrderIdAsync(orderId);
    }

    public async Task AddAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Height = dto.Height,
            Width = dto.Width,
            Length = dto.Length
        };
        await _repository.AddProductAsync(product);
    }

    public async Task<bool> UpdateAsync(int productId, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null)
            return false;

        if (dto.Name is not null)
            product.Name = dto.Name;

        if (dto.Height.HasValue)
            product.Height = dto.Height.Value;

        if (dto.Width.HasValue)
            product.Width = dto.Width.Value;

        if (dto.Length.HasValue)
            product.Length = dto.Length.Value;

        await _repository.UpdateProductAsync(product);

        return true;
    }

    public async Task DeleteByOrderIdAsync(int orderId)
    {
        await _repository.DeleteProductsByOrderIdAsync(orderId);
    }
}