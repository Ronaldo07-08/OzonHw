namespace Domain;

public interface IProductService
{
    Task<long> Create(string name, decimal price, double weight, ProductType type, int warehouseNum, CancellationToken ct = default);
    Task<Product> GetProduct(long id, CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetFilteredProducts(
        ProductType? type,
        DateTime? createdAt,
        int? warehouseNum,
        int offset,
        int limit,
        CancellationToken ct = default);
    Task UpdateProduct(long id, string name, decimal price, double weight, ProductType type, int warehouseNum, CancellationToken ct = default);
}