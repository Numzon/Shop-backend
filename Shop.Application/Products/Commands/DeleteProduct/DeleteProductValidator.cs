using FluentValidation;

namespace Shop.Application.Products.Commands.DeleteProduct;
public sealed class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
	public DeleteProductValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}
