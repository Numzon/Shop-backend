using FluentValidation;
using Shop.Application.Category.Models;

namespace Shop.Application.Category.Commands.EditCategory;
public sealed class EditSubcategoryValidator : AbstractValidator<EditSubcategoryDto>
{
	public EditSubcategoryValidator()
	{
        RuleFor(x => x.Name).NotEmpty();
    }
}
