using FluentValidation;
using Presentation.Rest.Models;

namespace Presentation.Rest.Validation;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.WarehouseNum).GreaterThan(0);
    }
}
