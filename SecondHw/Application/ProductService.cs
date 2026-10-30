using Domain;

namespace Application
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<long> Create(string name, decimal price, double weight, ProductType type, int warehouseNum)
        {
            return await _repo.Create(name, price, weight, type, warehouseNum);
        }

        public async Task<List<Product>> GetFilteredProducts(ProductType? type, DateTime? createdAt, int? warehouseNum, int offset, int limit)
        {
            return await _repo.GetFilteredProducts(type, createdAt, warehouseNum, offset, limit);
        }

        public async Task<Product> GetProduct(long id)
        {
            return await _repo.GetProduct(id);
        }
    }
}
