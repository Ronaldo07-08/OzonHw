using Domain;

namespace Presentation.Rest.Models
{

    public record GetProductResponse(
        long Id,
        string Name,
        decimal Price,
        double Weight,
        ProductType Type,
        DateTime CreatedAt,
        int WarehouseNum);
}
