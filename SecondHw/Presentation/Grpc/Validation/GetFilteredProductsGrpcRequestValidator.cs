using FluentValidation;
using Google.Protobuf.WellKnownTypes;

namespace Presentation.Grpc.Validation
{
    public class GetFilteredProductsGrpcRequestValidator : AbstractValidator<GetFilteredProductsGrpcRequest>
    {
        public GetFilteredProductsGrpcRequestValidator()
        {
            RuleFor(request => request.CreatedAt)
                .Must(date => date.ToDateTime().TimeOfDay == TimeSpan.Zero)
                .When(request => request.CreatedAt is not null)
                .WithMessage("Дата должна быть без времени, например 2026-10-20T00:00:00");

            RuleFor(request => request.Type)
                .IsInEnum()
                .When(request => request.HasType)
                .WithMessage("Неизвестный товара");

            RuleFor(request => request.WarehouseNum)
                .GreaterThan(0)
                .When(request => request.HasWarehouseNum)
                .WithMessage("Номер скалада должен быть больше 0");

            RuleFor(request => request.Offset)
                .GreaterThanOrEqualTo(0).WithMessage("Offset должен быть положительным или 0");

            RuleFor(request => request.Limit)
                .GreaterThan(0).WithMessage("Limit должен быть больше 0");
        }

    }
}
