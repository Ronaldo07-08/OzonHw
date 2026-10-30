using Domain;
using Microsoft.AspNetCore.Mvc;
using Presentation.Rest.Models;

namespace Presentation.Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] CreateProductRequest? request,
            CancellationToken cancellationToken)
        {
            var productId = await _service.Create(
                request.Name,
                request.Price,
                request.Weight,
                request.Type,
                request.WarehouseNum);

            return CreatedAtAction(nameof(GetProduct), new { id = productId }, productId);
        }


        [HttpGet("FilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts(
            [FromQuery] GetFilteredProductsRequest? request,
            CancellationToken cancellationToken
            )
        {
            var filteredProducts = await _service.GetFilteredProducts(
                request.Type,
                request.Date,
                request.WarehouseNum,
                request.Offset,
                request.Limit);

            var dto = filteredProducts.Select(
                p => new GetProductResponse(
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Weight,
                    p.Type,
                    p.CreatedAt,
                    p.WarehouseNum));

            return Ok(dto);
        }


        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _service.GetProduct(id);
            var dto = new GetProductResponse(
                product.Id,
                product.Name,
                product.Price,
                product.Weight,
                product.Type,
                product.CreatedAt,
                product.WarehouseNum);

            return Ok(dto);
        }
    }
}
