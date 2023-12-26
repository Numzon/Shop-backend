using FluentValidation;
using Shop.Application.SpecificationTypes.Commands.CreateSpecificationType;

namespace Shop.Application.SpecificationTypes.Commands.CreateCategory;
public sealed class CreateSpecificationTypeValidator : AbstractValidator<CreateSpecificationTypeCommand>
{
    public CreateSpecificationTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleForEach(x => x.Subtypes).SetValidator(new CreateSubspecificationTypeValidator());
    }
}