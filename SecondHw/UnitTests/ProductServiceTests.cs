using Application;
using Domain;
using Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace UnitTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_repoMock.Object);
    }

    [Fact]
    public async Task Create_ShouldCallRepositoryAndReturnNewId()
    {
        _repoMock
            .Setup(r => r.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(42L);

        var id = await _sut.Create("Laptop", 1500m, 2.1, ProductType.Tech, 1);

        id.Should().Be(42L);
        _repoMock.Verify(r => r.Create(
            It.Is<Product>(p => p.Name == "Laptop" && p.Price == 1500m && p.Type == ProductType.Tech),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ShouldReturnProduct()
    {
        var expected = new Product(1, "Milk", 100m, 1.0, ProductType.Groceries, DateTime.UtcNow, 2);
        _repoMock.Setup(r => r.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var actual = await _sut.GetProduct(1);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetProduct_WhenNotFound_ShouldThrowCannotGetProductException()
    {
        _repoMock.Setup(r => r.GetById(99, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        var act = async () => await _sut.GetProduct(99);

        await act.Should().ThrowAsync<CannotGetProductException>();
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ShouldThrowCannotUpdateProductException()
    {
        _repoMock
            .Setup(r => r.Update(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var act = async () => await _sut.UpdateProduct(99, "New", 10m, 1.0, ProductType.General, 1);

        await act.Should().ThrowAsync<CannotUpdateProductException>();
    }
}