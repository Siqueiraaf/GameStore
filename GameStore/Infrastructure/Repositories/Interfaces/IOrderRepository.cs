using GameStore.PackingService.Core.Models;

namespace GameStore.PackingService.Infrastructure.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<IEnumerable<Order>> GetOrdersByOrderIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task AddOrderAsync(Order order);
    Task AddProductAsync(Product product);
    Task DeleteOrderAsync(int id);
}
