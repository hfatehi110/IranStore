using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required")
                .NotNull()
                .MaximumLength(10).WithMessage("{UserName} Must Not Exceed 50 char");

            RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be grater than Zero");
        }
    }
}
