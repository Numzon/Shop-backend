using Shop.Application.Authentication.Commands.GenerateToken;

namespace Application.UnitTests.Authentication.Validators;
public sealed class RefreshTokenCommandValidationTests
{
    private readonly Fixture _fixture;

    public RefreshTokenCommandValidationTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Validate_TokenPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = _fixture.Build<RefreshTokenCommand>().Without(x => x.Token).Create();
        var validator = new RefreshTokenCommandValidation();

        var result = validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_RefreshTokenPropertyIsNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = _fixture.Build<RefreshTokenCommand>().Without(x => x.RefreshToken).Create();
        var validator = new RefreshTokenCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1);
    }

    [Fact]
    public void Validate_AllPropertiesAreNullOrEmpty_IsValidEqualFalseErrorListIsNotEmpty()
    {
        var model = new RefreshTokenCommand();
        var validator = new RefreshTokenCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty().And.HaveCount(2);
    }

    [Fact]
    public void Validate_AllPropertiesAreValid_IsValidEqualTrueErrorListIsEmpty()
    {
        var model = _fixture.Create<RefreshTokenCommand>();
        var validator = new RefreshTokenCommandValidation();

        var result = validator.Validate(model);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }
}
