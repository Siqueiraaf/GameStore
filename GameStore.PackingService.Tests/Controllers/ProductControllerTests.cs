using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.Controllers;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services;
using GameStore.PackingService.Features.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GameStore.PackingService.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _controller = new ProductController(_productServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfProducts()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Produto A" },
            new() { Id = 2, Name = "Produto B" }
        };
        _productServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count());
    }

    [Fact]
    public async Task GetByOrderId_ReturnsProductsByOrder()
    {
        int orderId = 5;
        var products = new List<Product>
        {
            new() { Id = 1, OrderId = orderId, Name = "Produto X" }
        };
        _productServiceMock.Setup(s => s.GetByOrderIdAsync(orderId)).ReturnsAsync(products);

        var result = await _controller.GetByOrderId(orderId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Single(returnedProducts);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var dto = new CreateProductDto("Produto Y", 10, 20, 30);
        _productServiceMock.Setup(s => s.AddAsync(dto)).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetAll), createdAt.ActionName);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var dto = new UpdateProductDto("Produto Atualizado", 1, 1, 1);
        _productServiceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenNotFound()
    {
        var dto = new UpdateProductDto("Não existe", 1, 1, 1);
        _productServiceMock.Setup(s => s.UpdateAsync(99, dto)).ReturnsAsync(false);

        var result = await _controller.Update(99, dto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteByOrderId_ReturnsNoContent()
    {
        int orderId = 10;
        _productServiceMock.Setup(s => s.DeleteByOrderIdAsync(orderId)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteByOrderId(orderId);

        Assert.IsType<NoContentResult>(result);
    }
}
