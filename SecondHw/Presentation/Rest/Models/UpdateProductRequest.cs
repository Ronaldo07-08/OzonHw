using Domain;

namespace Presentation.Rest.Models;

public record UpdateProductRequest(
    string Name,
    decimal Price,
    double Weight,
    ProductType Type,
    int WarehouseNum);