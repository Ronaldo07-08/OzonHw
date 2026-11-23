using Domain;
using Domain.Exceptions;

namespace Application;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<long> Create(
        string name,
        decimal price,
        double weight,
        ProductType type,
        int warehouseNum,
        CancellationToken ct = default)
    {
        var productToCreate = new Product(
            id: 0,
            name: name,
            price: price,
            weight: weight,
            type: type,
            createdAt: DateTime.UtcNow,
            warehouseNum: warehouseNum);

        return await _repo.Create(productToCreate, ct);
    }

    public async Task<IReadOnlyList<Product>> GetFilteredProducts(
        ProductType? type,
        DateTime? createdAt,
        int? warehouseNum,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        return await _repo.GetFiltered(type, createdAt, warehouseNum, offset, limit, ct);
    }

    public async Task<Product> GetProduct(long id, CancellationToken ct = default)
    {
        var product = await _repo.GetById(id, ct);
        if (product is null)
        {
            throw new CannotGetProductException($"Товар с Id = {id} не найден.");
        }

        return product;
    }

    public async Task UpdateProduct(
        long id,
        string name,
        decimal price,
        double weight,
        ProductType type,
        int warehouseNum,
        CancellationToken ct = default)
    {
        var productToUpdate = new Product(
            id: id,
            name: name,
            price: price,
            weight: weight,
            type: type,
            createdAt: default,
            warehouseNum: warehouseNum);

        var isUpdated = await _repo.Update(productToUpdate, ct);
        if (!isUpdated)
        {
            throw new CannotUpdateProductException($"Товар с Id = {id} не найден для обновления.");
        }
    }
}