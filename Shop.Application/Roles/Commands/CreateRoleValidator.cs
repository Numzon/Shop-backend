using FluentValidation;

namespace Shop.Application.Roles.Commands;
public sealed class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
	public CreateRoleValidator()
	{
		RuleFor(c => c.Name).NotEmpty();	
	}
}
