using FluentValidation;

namespace Shop.Application.Category.Commands.EditCategory;
public sealed class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
{
    public EditCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleForEach(x => x.Subcategories).SetValidator(new EditSubcategoryValidator());
    }
}
