using FluentValidation;

namespace Shop.Application.Users.Commands.ChangeEmail;
public sealed class ChangeEmailValidation : AbstractValidator<ChangeEmailCommand>
{
	public ChangeEmailValidation()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Email).NotEmpty();	
		RuleFor(x => x.NewEmail).NotEmpty().NotEqual(x => x.Email);
		RuleFor(x => x.Password).NotEmpty();
	}
}
