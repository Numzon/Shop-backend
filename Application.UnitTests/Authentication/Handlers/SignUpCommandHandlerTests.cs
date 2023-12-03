using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Handlers;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Factories;
using Shop.Application.Common.Interfaces;

namespace Application.UnitTests.Authentication.Handlers;
public sealed class SignUpCommandHandlerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IIdentityService> _identityService;

    public SignUpCommandHandlerTests()
    {
        _fixture = new Fixture();
        _identityService = new Mock<IIdentityService>();
    }

    [Fact]
    public async Task Handle_IdentityServiceInvalidPayloadResult_ReturnsIdentityServiceResult()
    {
        var resultObject = AuthResultDtoFactory.InvalidPayloadError();
        var requestObject = _fixture.Create<SignUpCommand>();
        _identityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(resultObject);
        var handler = new SignUpCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().Be(resultObject.Success);
        result.Token.Should().Be(resultObject.Token);
    }

    [Fact]
    public async Task Handle_IdentityServiceServerResultResult_ReturnsIdentityServiceResult()
    {
        var resultObject = AuthResultDtoFactory.ServerError();
        var requestObject = _fixture.Create<SignUpCommand>();
        _identityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(resultObject);
        var handler = new SignUpCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().Be(resultObject.Success);
        result.Token.Should().Be(resultObject.Token);
    }

    [Fact]
    public async Task Handle_IdentityServiceEmailInUserResult_ReturnsIdentityServiceResult()
    {
        var resultObject = AuthResultDtoFactory.EmailInUseError();
        var requestObject = _fixture.Create<SignUpCommand>();
        _identityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(resultObject);
        var handler = new SignUpCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().Be(resultObject.Success);
        result.Token.Should().Be(resultObject.Token);
    }

    [Fact]
    public async Task Handle_IdentityServiceSuccessResult_ReturnsIdentityServiceResult()
    {
        var resultObject = _fixture.Build<AuthResultDto>().With(x => x.Success, true).Create();
        var requestObject = _fixture.Create<SignUpCommand>();
        _identityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(resultObject);
        var handler = new SignUpCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().BeTrue();
        result.Token.Should().Be(resultObject.Token);
    }
}
