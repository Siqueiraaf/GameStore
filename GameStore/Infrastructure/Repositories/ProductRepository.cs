using Dapper;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Infrastructure.Data;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;

namespace GameStore.PackingService.Infrastructure.Repositories;

public class ProductRepository(GameStoreContext context) : IProductRepository
{
    private readonly GameStoreContext _context = context;

    public async Task AddProductAsync(Product product)
    {
        var query = @"
            INSERT INTO Product (Id, Height, Width, Length, OrderId)
            VALUES (@Id, @Heigth, @Width, @Length, @OrderId)";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, product);
    }

    public async Task DeleteProductsByOrderIdAsync(int orderId)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Products WHERE OrderId = @OrderId",
            new { OrderId = orderId });
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        var sql = "SELECT * FROM Products WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = productId });
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var query = "SELECT * FROM Products";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Product>(query);
    }

    public async Task<IEnumerable<Product>> GetProductsByOrderIdAsync(int orderId)
    {
        var query = "SELECT * FROM Products WHERE OrderId = @orderId";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Product>(query, new { orderId });
    }

    public async Task UpdateProductAsync(Product product)
    {
        var sql = @"
            UPDATE Products
            SET Name = @Name,
                Height = @Height,
                Width = @Width,
                Length = @Length
            WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            product.Name,
            product.Height,
            product.Width,
            product.Length,
            product.Id
        });
    }
}
