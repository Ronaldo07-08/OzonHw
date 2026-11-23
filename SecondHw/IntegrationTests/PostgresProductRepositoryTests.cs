using Domain;
using FluentAssertions;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace IntegrationTests;

public class PostgresProductRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    private PostgresProductRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var connectionString = _postgres.GetConnectionString();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = connectionString
            })
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(config);
        var provider = services.BuildServiceProvider();

        provider.MigrateDatabase();

        _repository = new PostgresProductRepository(connectionString);
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task CreateAndGetById_ShouldPersistAndReturnProduct()
    {
        var createdAt = new DateTime(2026, 11, 23, 12, 0, 0, DateTimeKind.Utc);
        var product = new Product(0, "Mechanical Keyboard", 12500m, 0.9, ProductType.Tech, createdAt, 3);

        var id = await _repository.Create(product);
        var loaded = await _repository.GetById(id);

        id.Should().BeGreaterThan(0);
        loaded.Should().NotBeNull();
        loaded!.Name.Should().Be("Mechanical Keyboard");
        loaded.Price.Should().Be(12500m);
        loaded.Type.Should().Be(ProductType.Tech);
        loaded.WarehouseNum.Should().Be(3);
    }

    [Fact]
    public async Task Update_ShouldModifyExistingProductFields()
    {
        var id = await _repository.Create(
            new Product(0, "Old Name", 100m, 1.0, ProductType.General, DateTime.UtcNow, 1));

        var updated = await _repository.Update(
            new Product(id, "Updated Name", 250m, 2.5, ProductType.Chemicals, DateTime.UtcNow, 5));
        var loaded = await _repository.GetById(id);

        updated.Should().BeTrue();
        loaded!.Name.Should().Be("Updated Name");
        loaded.Price.Should().Be(250m);
        loaded.Weight.Should().Be(2.5);
        loaded.Type.Should().Be(ProductType.Chemicals);
        loaded.WarehouseNum.Should().Be(5);
    }

    [Fact]
    public async Task GetFiltered_ShouldApplyFiltersAndPagination()
    {
        var targetDate = new DateTime(2026, 10, 15, 10, 30, 0, DateTimeKind.Utc);
        await _repository.Create(new Product(0, "Item 1", 10m, 1.0, ProductType.Tech, targetDate, 7));
        await _repository.Create(new Product(0, "Item 2", 20m, 1.0, ProductType.Tech, targetDate, 7));
        await _repository.Create(new Product(0, "Item 3", 30m, 1.0, ProductType.Groceries, targetDate, 7));

        var result = await _repository.GetFiltered(
            type: ProductType.Tech,
            createdAt: targetDate.Date,
            warehouseNum: 7,
            offset: 1,
            limit: 10);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Item 2");
    }
}