using FluentValidation;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Commands.EditSpecificationType;
public sealed class EditSubspecificationTypeValidator : AbstractValidator<EditSubspecificationTypeDto>
{
	public EditSubspecificationTypeValidator()
	{
        RuleFor(x => x.Name).NotEmpty();
    }
}
