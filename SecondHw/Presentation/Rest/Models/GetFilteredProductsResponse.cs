namespace Presentation.Rest.Models
{
    public record GetFilteredProductsResponse
    {
        public List<GetProductResponse>? FilteredProducts;
    }
}
