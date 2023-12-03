using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Handlers;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Factories;
using Shop.Application.Common.Interfaces;

namespace Application.UnitTests.Authentication.Handlers;
public sealed class RefreshTokenCommandHandlerTests
{
	private readonly Fixture _fixture;
	private readonly Mock<IIdentityService> _identityService;

	public RefreshTokenCommandHandlerTests()
	{
		_fixture = new Fixture();
		_identityService = new Mock<IIdentityService>();
    }

	[Fact]
	public async Task Handle_IdentityServiceInvalidTokenResult_ReturnsIdentityServiceResult()
	{
		var resultObject = AuthResultDtoFactory.InvalidTokensError();
		var requestTObject = _fixture.Create<RefreshTokenCommand>();
		_identityService.Setup(x => x.VerifyAndGenerateToken(It.IsAny<RefreshTokenCommand>())).ReturnsAsync(resultObject);
		var handler = new RefreshTokenCommandHandler(_identityService.Object);

		var result = await handler.Handle(requestTObject, default);

		result.Should().NotBeNull();	
		result.RefreshToken.Should().Be(resultObject.RefreshToken);
		result.Success.Should().Be(resultObject.Success);
		result.Token.Should().Be(resultObject.Token);
	}

    [Fact]
    public async Task Handle_IdentityServiceServerResultResult_ReturnsIdentityServiceResult()
    {
        var resultObject = AuthResultDtoFactory.ServerError();
        var requestTObject = _fixture.Create<RefreshTokenCommand>();
        _identityService.Setup(x => x.VerifyAndGenerateToken(It.IsAny<RefreshTokenCommand>())).ReturnsAsync(resultObject);
        var handler = new RefreshTokenCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestTObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().Be(resultObject.Success);
        result.Token.Should().Be(resultObject.Token);
    }

    [Fact]
    public async Task Handle_IdentityServiceExpiredTokenResult_ReturnsIdentityServiceResult()
    {
        var resultObject = AuthResultDtoFactory.ExpiredTokenError();
        var requestTObject = _fixture.Create<RefreshTokenCommand>();
        _identityService.Setup(x => x.VerifyAndGenerateToken(It.IsAny<RefreshTokenCommand>())).ReturnsAsync(resultObject);
        var handler = new RefreshTokenCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestTObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().Be(resultObject.Success);
        result.Token.Should().Be(resultObject.Token);
    }

    [Fact]
    public async Task Handle_IdentityServiceSuccessResult_ReturnsIdentityServiceResult()
    {
        var resultObject = _fixture.Build<AuthResultDto>().With(x => x.Success, true).Create();
        var requestTObject = _fixture.Create<RefreshTokenCommand>();
        _identityService.Setup(x => x.VerifyAndGenerateToken(It.IsAny<RefreshTokenCommand>())).ReturnsAsync(resultObject);
        var handler = new RefreshTokenCommandHandler(_identityService.Object);

        var result = await handler.Handle(requestTObject, default);

        result.Should().NotBeNull();
        result.RefreshToken.Should().Be(resultObject.RefreshToken);
        result.Success.Should().BeTrue();
        result.Token.Should().Be(resultObject.Token);
    }
}
