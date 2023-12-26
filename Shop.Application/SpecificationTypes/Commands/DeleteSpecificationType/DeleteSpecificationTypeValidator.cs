using FluentValidation;

namespace Shop.Application.SpecificationTypes.Commands.DeleteSpecificationType;
public sealed class DeleteSpecificationTypeValidator : AbstractValidator<DeleteSpecificationTypeCommand>
{
	public DeleteSpecificationTypeValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}
