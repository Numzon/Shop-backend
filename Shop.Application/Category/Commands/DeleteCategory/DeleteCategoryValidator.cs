using FluentValidation;

namespace Shop.Application.Category.Commands.DeleteCategory;
public sealed class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
	public DeleteCategoryValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}
