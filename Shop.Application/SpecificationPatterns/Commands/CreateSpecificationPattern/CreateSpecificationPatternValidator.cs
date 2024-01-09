using FluentValidation;

namespace Shop.Application.SpecificationPatterns.Commands.CreateSpecificationPattern;
public sealed class CreateSpecificationPatternValidator : AbstractValidator<CreateSpecificationPatternCommand>
{
	public CreateSpecificationPatternValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleForEach(x => x.Types).SetValidator(new SpecificationTypeIdDtoValidator());
	}
}
