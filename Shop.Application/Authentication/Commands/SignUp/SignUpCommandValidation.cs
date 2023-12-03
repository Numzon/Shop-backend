using FluentValidation;

namespace Shop.Application.Authentication.Commands.SignUp;
public class SignUpCommandValidation : AbstractValidator<SignUpCommand>
{
	public SignUpCommandValidation()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Password).NotEmpty();
		RuleFor(x => x.RepeatPassword).NotEmpty().Equal(x => x.Password);
	}
}
