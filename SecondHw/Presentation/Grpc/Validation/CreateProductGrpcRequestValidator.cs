    using FluentValidation;

    namespace Presentation.Grpc.Validation
    {
        public class CreateProductGrpcRequestValidator : AbstractValidator<CreateProductGrpcRequest>
        {
            public CreateProductGrpcRequestValidator()
            {
                RuleFor(request => request.Name)
                    .NotEmpty().WithMessage("Имя обязательно")
                    .MaximumLength(100);

                RuleFor(request => request.Price)
                     .GreaterThan(0).WithMessage("Цена должна быть больше 0");

                RuleFor(request => request.Type)
                     .IsInEnum().WithMessage("Должен быть задан вид товара");

                RuleFor(request => request.WarehouseNum)
                     .NotEmpty().WithMessage("Должен быть задан номер склада")
                     .GreaterThan(0).WithMessage("Номер скалада должен быть больше 0");

                RuleFor(request => request.Weight)
                     .NotEmpty().WithMessage("Должен быть задан вес")
                     .GreaterThan(0).WithMessage("Вес должен быть больше 0");
            }

        }
    }
