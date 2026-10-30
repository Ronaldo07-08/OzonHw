using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IProductService
    {
        Task<long> Create(
                    string name, decimal price, double weight, ProductType type, int warehouseNum);
        Task<List<Product>> GetFilteredProducts(
            ProductType? type, DateTime? createdAt, int? warehouseNum, int offset, int limit);

        Task<Product> GetProduct(long id);

    }
}
