using FluentValidation;
using Shop.Application.Cart.Commands.ReduceQuantity;

namespace Shop.Application.Cart.Commands.SetQuantity;
public sealed class SetQuantityValidator : AbstractValidator<SetQuantityCommand>
{
    public SetQuantityValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.Quantity).NotEmpty();
    }
}
