using System.Net.Mime;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.PackingService.Features.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("/packing")]
[ApiController]
public class PackController(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    PackService packService) : ControllerBase
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly PackService _packService = packService;

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(List<PackingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<PackingDto>>> Post([FromBody] List<Order> orders)
    {
        foreach (var order in orders)
        {
            await _orderRepository.AddOrderAsync(order);

            foreach (var product in order.Products)
            {
                product.OrderId = order.Id;
                await _productRepository.AddProductAsync(product);
            }
        }

        var result = orders.Select(order => _packService.PackOrder(order)).ToList();
        return Ok(result);
    }

    [HttpGet("orders")]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        foreach (var order in orders)
        {
            order.Products = (List<Product>)await _productRepository.GetProductsByOrderIdAsync(order.Id);
        }
        return Ok(orders);
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(PackingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PackingDto>> GetPackedOrder(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            return NotFound($"Pedido {orderId} não encontrado.");

        order.Products = (List<Product>)await _productRepository.GetProductsByOrderIdAsync(orderId);
        var result = _packService.PackOrder(order);
        return Ok(result);
    }

    [HttpDelete("{orderId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            return NotFound();

        await _productRepository.DeleteProductsByOrderIdAsync(orderId);
        await _orderRepository.DeleteOrderAsync(orderId);
        return NoContent();
    }
}
