using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(X => X.UserName).MaximumLength(50).WithMessage("{UserName} Has Max Length 50")
                .NotNull().NotEmpty();

            RuleFor(x=>x.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is Required");

            RuleFor(x => x.TotalPrice).GreaterThan(0).NotEmpty().NotNull();
        }
    }
}
