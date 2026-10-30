using Domain;
namespace Presentation.Rest.Models
{
    public record CreateProductRequest(
        string Name, decimal Price, double Weight, ProductType Type, int WarehouseNum);
}
