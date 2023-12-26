using FluentValidation;

namespace Shop.Application.Category.Commands.CreateCategory;
public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleForEach(x => x.Subcategories).SetValidator(new CreateSubcategoryValidator());
    }
}