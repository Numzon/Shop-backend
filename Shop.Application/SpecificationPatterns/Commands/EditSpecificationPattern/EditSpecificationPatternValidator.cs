using FluentValidation;

namespace Shop.Application.SpecificationPatterns.Commands.EditSpecificationPattern;
public sealed class EditSpecificationPatternValidator : AbstractValidator<EditSpecificationPatternCommand>
{
	public EditSpecificationPatternValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name).NotEmpty();
		RuleForEach(x => x.Types).SetValidator(new SimpleSpecificationPatternSpecificationTypeValidator());
	}
}
