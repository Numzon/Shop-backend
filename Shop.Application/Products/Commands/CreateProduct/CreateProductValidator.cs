using FluentValidation;

namespace Shop.Application.Products.Commands.CreateProduct;
public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
	public CreateProductValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
	}
}
