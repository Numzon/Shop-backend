using FluentValidation;

namespace Shop.Application.SpecificationPatterns.Commands.DeleteSpecificationPattern;
public sealed class DeleteSpecificationPatternValidator : AbstractValidator<DeleteSpecificationPatternCommand>
{
	public DeleteSpecificationPatternValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}
