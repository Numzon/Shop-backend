using FluentValidation;

namespace Shop.Application.SpecificationTypes.Commands.EditSpecificationType;
public sealed class EditSpecificationTypeValidator : AbstractValidator<EditSpecificationTypeCommand>
{
    public EditSpecificationTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleForEach(x => x.Subtypes).SetValidator(new EditSubspecificationTypeValidator());
    }
}
