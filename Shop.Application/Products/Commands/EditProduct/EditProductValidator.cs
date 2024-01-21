using FluentValidation;

namespace Shop.Application.Products.Commands.EditProduct;
public sealed class EditProductValidator : AbstractValidator<EditProductCommand>
{
	public EditProductValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();	
	}
}
