using FluentValidation;
using PaycoreProject.Model;

namespace PaycoreProject.Validators
{
    public class UserValidator : AbstractValidator<RegisterRequest>
    {
        public UserValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Can not be Empty")
                .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
                .Length(0, 50).WithMessage("Enter a value between 0 and 50.");

            RuleFor(c => c.Surname)
               .NotEmpty().WithMessage("Cannot be Empty")
                .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
                .Length(0, 50).WithMessage("Enter a value between 0 and 50.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Cannot be Empty")
                .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
               .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(c => c.Address)
           .NotEmpty().WithMessage("Cannot be Empty")
           .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
           .Length(0, 200).WithMessage("Enter a value between 0 and 200.");

            
            RuleFor(c => c.Password)
         .NotEmpty().WithMessage("Cannot be Empty")
         .NotNull().WithMessage("SystemMessage.NOT_EMPTY")
         .Length(8, 20).WithMessage("Enter a value between 8 and 20.");
        }
    }
}