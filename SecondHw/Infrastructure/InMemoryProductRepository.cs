using Domain;
using Domain.Exceptions;
using C = System.Collections.Concurrent;

namespace Infrastructure
{
    public class InMemoryProductRepository : IProductRepository
    {
        private long _currentId = 0;
        private readonly C.ConcurrentDictionary<long, Product> _repo = new C.ConcurrentDictionary<long, Product>();

        public Task<long> Create(Product product, CancellationToken cancellationToken)
        {
            if (_repo.TryAdd(product.Id, product))
                return Task.FromResult(product.Id);
            else
                return Task.FromException<long>(new CannotCreateProductException($"Ошибка создания Product с Id - {product.Id}"));
        }

        public Task<IReadOnlyList<Product>> GetFiltered(
           ProductType? type,
           DateTime? createdAt,
           int? warehouseNum,
           int offset,
           int limit,
           CancellationToken cancellationToken)
        {

            IReadOnlyList<Product> result = _repo
                .Where(pair =>
                    (type == null || pair.Value.Type == type) &&
                    (createdAt == null || pair.Value.CreatedAt.Date == createdAt.Value.Date) &&
                    (warehouseNum == null || pair.Value.WarehouseNum == warehouseNum))
                .OrderBy(pair => pair.Key)
                .Skip(offset)
                .Take(limit)
                .Select(pair => pair.Value)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<Product> GetById(long id, CancellationToken cancellationToken)
        {
            Product product;
            if (_repo.TryGetValue(id, out product))
                return Task.FromResult(product);
            else
                return Task.FromException<Product>(new CannotGetProductException($"Нет продуктов с id: {id}"));
        }

        public Task<bool> Update(Product product, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(product);
            ct.ThrowIfCancellationRequested();

            if (!_repo.TryGetValue(product.Id, out var current))
                return Task.FromResult(false);

            return Task.FromResult(_repo.TryUpdate(product.Id, product, current));
        }
    }
}
