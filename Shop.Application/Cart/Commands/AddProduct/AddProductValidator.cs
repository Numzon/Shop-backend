using FluentValidation;

namespace Shop.Application.Cart.Commands.AddProduct;
public sealed class AddProductValidator : AbstractValidator<AddProductToCartCommand>
{
	public AddProductValidator()
	{
		RuleFor(x => x.CartId).NotNull();
		RuleFor(x => x.ProductId).NotEmpty();
	}
}
