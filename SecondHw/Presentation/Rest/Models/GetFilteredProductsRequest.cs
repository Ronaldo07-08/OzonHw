using Domain;

namespace Presentation.Rest.Models
{
    public record GetFilteredProductsRequest(
        DateTime? Date, ProductType? Type, int? WarehouseNum, int Offset, int Limit);
}
