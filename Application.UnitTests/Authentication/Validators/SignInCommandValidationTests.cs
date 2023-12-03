using Shop.Application.Authentication.Commands.SignIn;
using System.Net.Mail;

namespace Application.UnitTests.Authentication.Validators;
public sealed class SignInCommandValidationTests 
{
    private readonly Fixture _fixture;

    public SignInCommandValidationTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Validate_EmailPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = _fixture.Build<SignInCommand>().Without(x => x.Email).Create();
        var validator = new SignInCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_EmailPropertyIsIncorrect_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = _fixture.Build<SignInCommand>().Create();
        var validator = new SignInCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_PasswordPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var address = _fixture.Create<MailAddress>().Address;
        var model = _fixture.Build<SignInCommand>().With(x => x.Email, address).Without(x => x.Password).Create();
        var validator = new SignInCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_AllPropertiesAreNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = new SignInCommand();
        var validator = new SignInCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(2);
    }

    [Fact]
    public void Validate_AllPropertiesAreValid_IsValidEqualTrueErrorListIsEmpty()
    {
        var address = _fixture.Create<MailAddress>().Address;
        var model = _fixture.Build<SignInCommand>().With(x => x.Email, address).Create();
        var validator = new SignInCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }
}
