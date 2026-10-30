using Domain;
using FluentValidation;
using Presentation.Rest.Models;

namespace Presentation.Rest.Validation
{
    public class GetFilteredProductsRequestValidator : AbstractValidator<GetFilteredProductsRequest>
    {

        public GetFilteredProductsRequestValidator()
        {

            RuleFor(request => request.Date)
                .Must(date => date is null || date.Value.TimeOfDay == TimeSpan.Zero).WithMessage("Дата должна быть без времени, например 2026-10-20T00:00:00");

            RuleFor(request => request.Type)
                .IsInEnum();

            RuleFor(request => request.WarehouseNum)
                .Must(wNum => wNum is null || wNum > 0).WithMessage("Номер скалада должен быть больше 0");

            RuleFor(request => request.Offset)
                .GreaterThanOrEqualTo(0).WithMessage("Offset должен быть положительным или 0");

            RuleFor(request => request.Limit)
                .GreaterThan(0).WithMessage("Limit должен быть больше 0");

        }
    }
}
