using AutoMapper;
using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.Features.Commands.Product.Add;
using Moq;
using System.Net;

namespace JoygameProject.Tests.Tests.Product
{
    public class ProductAddCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<ICacheService> _cacheMock = new();

        private readonly ProductAddCommandHandler _handler;

        public ProductAddCommandHandlerTests()
        {
            _handler = new ProductAddCommandHandler(
                _mapperMock.Object,
                _uowMock.Object,
                _cacheMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldAddProduct_AndCacheIt_AndClearProductList()
        {
            // Arrange
            var request = new ProductAddCommandRequest
            {
                Name = "TestProduct",
                Price = 999.99f,
                ImageUrl = "https://google.com",
                CatId = 1
            };

            var productEntity = new Domain.Entities.Product
            {
                Id = 42,
                Name = "TestProduct",
                ImageUrl = "https://google.com",
                Price = 999.99f,
                CatId = 1,
                IsActive = true
            };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Product>(request)).Returns(productEntity);
            _uowMock.Setup(u => u.Write<Domain.Entities.Product>().Add(productEntity));
            _uowMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _cacheMock.Setup(c => c.SetAsync($"product-detail-{productEntity.Id}", productEntity, null)).Returns(Task.CompletedTask);
            _cacheMock.Setup(c => c.RemoveAsync("product-list")).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Status == HttpStatusCode.OK);
            Assert.Equal(42, result.Result);

            _mapperMock.Verify(m => m.Map<Domain.Entities.Product>(request), Times.Once);
            _uowMock.Verify(u => u.Write<Domain.Entities.Product>().Add(productEntity), Times.Once);
            _uowMock.Verify(u => u.SaveAsync(), Times.Once);
            _cacheMock.Verify(c => c.SetAsync($"product-detail-{productEntity.Id}", productEntity, null), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync("product-list"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldAddProductAndReturnId()
        {
            // Arrange
            var request = new ProductAddCommandRequest
            {
                Name = "Laptop",
                ImageUrl = "https://google.com",
                Price = 1000
            };

            var productEntity = new Domain.Entities.Product { Id = 1, Name = "Laptop", ImageUrl = "https://google.com", Price = 1000 };

            _mapperMock.Setup(x => x.Map<Domain.Entities.Product>(request)).Returns(productEntity);

            _uowMock.Setup(x => x.Write<Domain.Entities.Product>().Add(productEntity));
            _uowMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Status == HttpStatusCode.OK);
            Assert.Equal(1, result.Result);
        }
    }

}
