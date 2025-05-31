using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;
using Moq;

namespace GameStore.PackingService.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock = new();

        [Fact]
        public async Task GetAllAsync_ReturnsProducts()
        {
            _repositoryMock.Setup(r => r.GetProductsAsync())
                .ReturnsAsync(new List<Product> { new Product { Id = 1, Name = "Prod1" } });

            var service = new ProductService(_repositoryMock.Object);

            var products = await service.GetAllAsync();

            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task GetByOrderIdAsync_ReturnsProducts()
        {
            int orderId = 5;

            _repositoryMock.Setup(r => r.GetProductsByOrderIdAsync(orderId))
                .ReturnsAsync(new List<Product> { new Product { Id = 2, Name = "Prod2" } });

            var service = new ProductService(_repositoryMock.Object);

            var products = await service.GetByOrderIdAsync(orderId);

            Assert.NotEmpty(products);
            Assert.All(products, p => Assert.Equal(2, p.Id));
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAdd()
        {
            var dto = new CreateProductDto("IPod", 10, 10, 10);
            var service = new ProductService(_repositoryMock.Object);

            await service.AddAsync(dto);

            _repositoryMock.Verify(r => r.AddProductAsync(It.Is<Product>(p => p.Name == "IPod" && p.Height == 10)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenProductExists()
        {
            int productId = 1;
            var existingProduct = new Product { Id = productId, Name = "OldName", Height = 5, Width = 5, Length = 5 };
            var dto = new UpdateProductDto("NewName", 10, null, null);

            _repositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
            _repositoryMock.Setup(r => r.UpdateProductAsync(existingProduct)).Returns(Task.CompletedTask);

            var service = new ProductService(_repositoryMock.Object);

            var result = await service.UpdateAsync(productId, dto);

            Assert.True(result);
            Assert.Equal("NewName", existingProduct.Name);
            Assert.Equal(10, existingProduct.Height);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenProductNotFound()
        {
            int productId = 999;
            var dto = new UpdateProductDto(null, 10, null, null);

            _repositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

            var service = new ProductService(_repositoryMock.Object);

            var result = await service.UpdateAsync(productId, dto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteByOrderIdAsync_CallsRepositoryDelete()
        {
            int orderId = 3;

            _repositoryMock.Setup(r => r.DeleteProductsByOrderIdAsync(orderId)).Returns(Task.CompletedTask);

            var service = new ProductService(_repositoryMock.Object);

            await service.DeleteByOrderIdAsync(orderId);

            _repositoryMock.Verify(r => r.DeleteProductsByOrderIdAsync(orderId), Times.Once);
        }
    }
}
