using FluentValidation;
using Shop.Application.Category.Models;

namespace Shop.Application.Category.Commands.CreateCategory;
public sealed class CreateSubcategoryValidator : AbstractValidator<CreateSubcategoryDto>
{
    public CreateSubcategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
