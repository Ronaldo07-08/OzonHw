using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Presentation.Grpc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrpcClient.Controllers
{
    [ApiController]
    [Route("api/client/products")]
    public class ClientProductController : ControllerBase
    {
        private readonly ProductGrpc.ProductGrpcClient _grpcClient;

        public ClientProductController(ProductGrpc.ProductGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductGrpcRequest request)
        {
            try
            {
                var response = await _grpcClient.CreateProductAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return BadRequest(new { grpcStatus = ex.StatusCode.ToString(), error = ex.Status.Detail });
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            try
            {
                var response = await _grpcClient.GetProductAsync(new GetProductGrpcRequest { Id = id });
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return BadRequest(new { grpcStatus = ex.StatusCode.ToString(), error = ex.Status.Detail });
            }

        }

        public record Model(DateTime? date, GrpcProductType? type, int? warehouseNum, int offset, int limit);

        [HttpGet("FilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] Model request)
        {
            try
            {
                var grpcRequest = new GetFilteredProductsGrpcRequest
                {
                    Offset = request.offset,
                    Limit = request.limit
                };

                if (request.type.HasValue) grpcRequest.Type = request.type.Value;
                if (request.warehouseNum.HasValue) grpcRequest.WarehouseNum = request.warehouseNum.Value;
                if (request.date.HasValue) grpcRequest.CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(request.date.Value, DateTimeKind.Utc));

                var response = await _grpcClient.GetFilteredProductsAsync(grpcRequest);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return BadRequest(new { grpcStatus = ex.StatusCode.ToString(), error = ex.Status.Detail });
            }
        }
    }
}
