using Dapper;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Infrastructure.Data;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;

namespace GameStore.PackingService.Infrastructure.Repositories;

public class OrderRepository(GameStoreContext context) : IOrderRepository
{
    // injecao de dependencia.
    private readonly GameStoreContext _context = context;

    public async Task AddOrderAsync(Order order)
    {
        var query = "INSERT INTO Orders (Id) VALUES (@Id)";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, order);
    }

    public async Task AddProductAsync(Product product)
    {
        var query = "INSERT INTO Product (Id, Heigth, Width, Length, OrderId)" +
            "VALUES (@Id, @Heigth, @Width, @Length, @OrderId)";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, product);
    }

    public async Task DeleteOrderAsync(int id)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Orders WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<Order>("SELECT * FROM Orders");
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Order>(
            "SELECT * FROM Orders WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var query = "SELECT * FROM Pedidos";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Order>(query);
    }

    public async Task<IEnumerable<Order>> GetOrdersByOrderIdAsync(int orderId)
    {
        var query = "SELECT * From Products WHERE OrderId = @orderId";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Order>(query, new { orderId });
    }
}
