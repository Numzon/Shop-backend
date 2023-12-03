using FluentValidation;

namespace Application.UnitTests.Common.Models;
public class RandomRequestValidation : AbstractValidator<RandomRequest>
{
	public RandomRequestValidation()
	{
		RuleFor(x => x.RandomStringProperty).NotEmpty();
	}
}
