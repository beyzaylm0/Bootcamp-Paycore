using FluentValidation;
using PaycoreProject.Model;

namespace PaycoreProject.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(c => c.ProductName)
                .NotEmpty().WithMessage("Can not be Empty")
                .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
                .Length(0, 100).WithMessage("Enter a value between 0 and 100.");

            RuleFor(c => c.Description)
               .NotEmpty().WithMessage("Cannot be Empty")
                .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
                .Length(0, 500).WithMessage("Enter a value between 0 and 500.");

            RuleFor(c => c.Color)
           .NotEmpty().WithMessage("Cannot be Empty")
                .NotNull().WithMessage("SystemMessage.NOT_EMPTY");

            RuleFor(c => c.Brand)
           .NotEmpty().WithMessage("Cannot be Empty")
           .NotNull().WithMessage("SystemMessage.NOT_EMPTY");


            RuleFor(c => c.Price)
           .NotEmpty().WithMessage("Cannot be Empty")
           .NotNull().WithMessage("SystemMessage.NOT_EMPTY");
        }
    }
}
