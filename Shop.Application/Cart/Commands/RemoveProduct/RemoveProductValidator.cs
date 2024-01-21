using FluentValidation;

namespace Shop.Application.Cart.Commands.RemoveProduct;
public sealed class RemoveProductValidator : AbstractValidator<RemoveProductCommand>
{
    public RemoveProductValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
