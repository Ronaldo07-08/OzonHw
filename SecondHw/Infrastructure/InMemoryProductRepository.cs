using Domain;
using Domain.Exceptions;
using C = System.Collections.Concurrent;

namespace Infrastructure
{
    public class InMemoryProductRepository : IProductRepository
    {
        private long _currentId = 0;
        private readonly C.ConcurrentDictionary<long, Product> _repo = new C.ConcurrentDictionary<long, Product>();

        public Task<long> Create(string name, decimal price, double weight, ProductType type, int warehouseNum)
        {
            var product = new Product(
                Interlocked.Increment(ref _currentId),
                name,
                price,
                weight,
                type,
                DateTime.UtcNow,
                warehouseNum);

            if (_repo.TryAdd(product.Id, product))
                return Task.FromResult(product.Id);
            else
                return Task.FromException<long>(new CannotCreateProductException($"Ошибка создания Product с Id - {product.Id}"));
        }

        public Task<List<Product>> GetFilteredProducts(ProductType? type, DateTime? createdAt, int? warehouseNum, int offset, int limit)
        {
            return Task.FromResult(_repo.Where(pair => type == null || pair.Value.Type == type)
                .Where(pair => createdAt == null || pair.Value.CreatedAt.Date == createdAt.Value.Date)
                .Where(pair => warehouseNum == null || pair.Value.WarehouseNum == warehouseNum)
                .OrderBy(pair => pair.Key)
                .Skip(offset)
                .Take(limit)
                .Select(pair => pair.Value)
                .ToList());
        }

        public Task<Product> GetProduct(long id)
        {
            Product product;
            if (_repo.TryGetValue(id, out product))
                return Task.FromResult(product);
            else
                return Task.FromException<Product>(new CannotGetProductException($"Нет продуктов с id: {id}"));
        }
    }
}
