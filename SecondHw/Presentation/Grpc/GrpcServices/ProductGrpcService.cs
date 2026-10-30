using Domain;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Presentation.Grpc;

namespace Presentation.Grpc.GrpcServices
{
    public class ProductGrpcService : ProductGrpc.ProductGrpcBase
    {
        private readonly IProductService _service;

        public ProductGrpcService(IProductService service)
        {
            _service = service;
        }

        public override async Task<CreateProductGrpcResponse> CreateProduct(CreateProductGrpcRequest request, ServerCallContext context)
        {
            var id = await _service.Create(
                request.Name,
                (decimal)request.Price,
                request.Weight,
                (ProductType)request.Type,
                request.WarehouseNum
            );

            return new CreateProductGrpcResponse{ Id = id };
        }

        public override async Task<GetFilteredProductsGrpcResponse> GetFilteredProducts(GetFilteredProductsGrpcRequest request, ServerCallContext context)
        {

            ProductType? requestType = request.HasType ? (ProductType)request.Type : null;
            int? requestWarehouseNum = request.HasWarehouseNum ? request.WarehouseNum : null;
            DateTime? requestCreatedAt = request.CreatedAt is not null ? request.CreatedAt.ToDateTime() : null;

            var filteredProducts = await _service.GetFilteredProducts(
                    requestType,
                    requestCreatedAt,
                    requestWarehouseNum,
                    request.Offset,
                    request.Limit
            );

            var response = new GetFilteredProductsGrpcResponse();
            response.FilteredProducts.AddRange(filteredProducts.Select(p => new GetProductGrpcResponse{
                Id = p.Id,
                Name = p.Name,
                Price = (double)p.Price,
                Type = (GrpcProductType)p.Type,
                CreatedAt = Timestamp.FromDateTime(p.CreatedAt),
                WarehouseNum = p.WarehouseNum,
                Weight = p.Weight
            }));

            return response;
        }

        public override async Task<GetProductGrpcResponse> GetProduct(GetProductGrpcRequest request, ServerCallContext context)
        {
            var product = await _service.GetProduct(request.Id);
            
            return new GetProductGrpcResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = (double)product.Price,
                Type = (GrpcProductType)product.Type,
                CreatedAt = Timestamp.FromDateTime(product.CreatedAt),
                WarehouseNum = product.WarehouseNum,
                Weight = product.Weight
            };
        }


    }
}
