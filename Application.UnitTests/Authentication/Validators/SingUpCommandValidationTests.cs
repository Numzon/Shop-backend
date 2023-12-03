using Shop.Application.Authentication.Commands.SignUp;
using System.Net.Mail;

namespace Application.UnitTests.Authentication.Validators;
public sealed class SingUpCommandValidationTests 
{
    private readonly Fixture _fixture;

    public SingUpCommandValidationTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Validate_EmailPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var password = _fixture.Create<string>();
        var model = _fixture.Build<SignUpCommand>()
            .With(x => x.Password, password)
            .With(x => x.RepeatPassword, password)
            .Without(x => x.Email).Create();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_EmailPropertyIsIncorrect_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var password = _fixture.Create<string>();
        var model = _fixture.Build<SignUpCommand>()
            .With(x => x.Password, password)
            .With(x => x.RepeatPassword, password)
            .Create();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_PasswordPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var address = _fixture.Create<MailAddress>().Address;
        var model = _fixture.Build<SignUpCommand>().With(x => x.Email, address).Without(x => x.Password).Create();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(2);
    }


    [Fact]
    public void Validate_RepeatPasswordPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var address = _fixture.Create<MailAddress>().Address;
        var model = _fixture.Build<SignUpCommand>().With(x => x.Email, address).Without(x => x.RepeatPassword).Create();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(2);
    }

    [Fact]
    public void Validate_RepeatPasswordPropertyDoesntMatchPasswordProperty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var address = _fixture.Create<MailAddress>().Address;
        var model = _fixture.Build<SignUpCommand>()
            .With(x => x.Email, address)
            .Create();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_AllPropertiesAreNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = new SignUpCommand();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(3);
    }

    [Fact]
    public void Validate_AllPropertiesAreValid_IsValidEqualTrueErrorListIsEmpty()
    {
        var address = _fixture.Create<MailAddress>().Address;
        var password = _fixture.Create<string>();
        var model = _fixture.Build<SignUpCommand>()
            .With(x => x.Email, address)
            .With(x => x.Password, password)
            .With(x => x.RepeatPassword, password)
            .Create();
        var validator = new SignUpCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }
}
