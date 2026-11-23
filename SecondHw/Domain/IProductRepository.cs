namespace Domain;

public interface IProductRepository
{
    Task<long> Create(Product product, CancellationToken ct = default);
    Task<Product?> GetById(long id, CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetFiltered(
        ProductType? type,
        DateTime? createdAt,
        int? warehouseNum,
        int offset,
        int limit,
        CancellationToken ct = default);
    Task<bool> Update(Product product, CancellationToken ct = default);
}