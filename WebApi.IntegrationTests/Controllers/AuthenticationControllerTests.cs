using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Factories;
using Shop.Domain.Constants;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Controllers;
public sealed class AuthenticationControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Fixture _fixture;

    public AuthenticationControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
        _fixture = new Fixture();
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }

    [Fact]
    public async Task SignUp_InvalidRequest_ThrowsValidationError()
    {
        var request = _fixture.Build<SignUpCommand>().Without(x => x.Email).Create();

        var response = await _client.PostAsync("api/authentication/sign-up", JsonContent.Create(request));

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var data = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull().And.HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task SignUp_EmailIsAlreadyInUser_ReturnsEmailIsAlreadyInUserError()
    {
        var request = GenerateValidSignUpCommand();
        _factory.IdentityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(AuthResultDtoFactory.EmailInUseError);

        var response = await _client.PostAsync("api/authentication/sign-up", JsonContent.Create(request));

        response.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeFalse();
        data.Errors.Should().NotBeNull().And.HaveCount(1).And.Contain(AuthenticationErrors.EmailIsAlreadyInUse);
    }

    [Fact]
    public async Task SignUp_UserCreationFailed_ReturnsAuthResultDtoObjectWithErrors()
    {
        var request = GenerateValidSignUpCommand();
        var response = _fixture.Build<AuthResultDto>().With(x => x.Success, false).With(x => x.Errors, _fixture.CreateMany<string>(10)).Create();
        _factory.IdentityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(response);

        var result = await _client.PostAsync("api/authentication/sign-up", JsonContent.Create(request));

        result.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await result.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeFalse();
        data.Errors.Should().NotBeNull().And.HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task SignUp_UserCreationFailed_ReturnsServerError()
    {
        var request = GenerateValidSignUpCommand();
        _factory.IdentityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(AuthResultDtoFactory.ServerError());

        var result = await _client.PostAsync("api/authentication/sign-up", JsonContent.Create(request));

        result.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await result.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeFalse();
        data.Errors.Should().NotBeNull().And.HaveCount(1).And.Contain(AuthenticationErrors.ServerError);
    }


    [Fact]
    public async Task SignUp_SignedIn_ReturnsTokens()
    {
        var request = GenerateValidSignUpCommand();
        var response = _fixture.Build<AuthResultDto>().With(x => x.Success, true).Create();
        _factory.IdentityService.Setup(x => x.SignUpUser(It.IsAny<SignUpCommand>())).ReturnsAsync(response);

        var result = await _client.PostAsync("api/authentication/sign-up", JsonContent.Create(request));

        result.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await result.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeTrue();
        data.RefreshToken.Should().NotBeNullOrEmpty();
        data.Token.Should().NotBeNullOrEmpty();
    }


    [Fact]
    public async Task SignIn_InvalidRequest_ThrowsValidationError()
    {
        var request = _fixture.Build<SignInCommand>().Without(x => x.Email).Create();

        var response = await _client.PostAsync("api/authentication/sign-in", JsonContent.Create(request));

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var data = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull().And.HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task SignIn_EmailIsAlreadyInUser_ReturnsEmailIsAlreadyInUserError()
    {
        var request = _fixture.Build<SignInCommand>().With(x =>x.Email, _fixture.Create<MailAddress>().Address).Create();
        _factory.IdentityService.Setup(x => x.SignInUser(It.IsAny<SignInCommand>())).ReturnsAsync(AuthResultDtoFactory.IncorrectEmailOrPasswordError);

        var response = await _client.PostAsync("api/authentication/sign-in", JsonContent.Create(request));

        response.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeFalse();
        data.Errors.Should().NotBeNull().And.HaveCount(1).And.Contain(AuthenticationErrors.IncorrectEmailOrPassword);
    }

    [Fact]
    public async Task SignIn_SignedIn_ReturnsTokens()
    {
        var request = _fixture.Build<SignInCommand>().With(x => x.Email, _fixture.Create<MailAddress>().Address).Create();
        var response = _fixture.Build<AuthResultDto>().With(x => x.Success, true).Create();
        _factory.IdentityService.Setup(x => x.SignInUser(It.IsAny<SignInCommand>())).ReturnsAsync(response);

        var result = await _client.PostAsync("api/authentication/sign-in", JsonContent.Create(request));

        result.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await result.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeTrue();
        data.RefreshToken.Should().NotBeNullOrEmpty();
        data.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RefreshToken_InvalidRequest_ThrowsValidationError()
    {
        var request = _fixture.Build<RefreshTokenCommand>().Without(x => x.Token).Create();

        var response = await _client.PostAsync("api/authentication/refresh-token", JsonContent.Create(request));

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var data = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull().And.HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task RefreshToken_EmailIsAlreadyInUser_ReturnsEmailIsAlreadyInUserError()
    {
        var request = _fixture.Create<RefreshTokenCommand>();
        _factory.IdentityService.Setup(x => x.VerifyAndGenerateToken(It.IsAny<RefreshTokenCommand>())).ReturnsAsync(AuthResultDtoFactory.ExpiredTokenError);

        var response = await _client.PostAsync("api/authentication/refresh-token", JsonContent.Create(request));

        response.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeFalse();
        data.Errors.Should().NotBeNull().And.HaveCount(1).And.Contain(AuthenticationErrors.ExpiredToken);
    }

    [Fact]
    public async Task RefreshToken_GeneratedNewToken_ReturnsTokens()
    {
        var request = _fixture.Create<RefreshTokenCommand>();
        var response = _fixture.Build<AuthResultDto>().With(x => x.Success, true).Create();
        _factory.IdentityService.Setup(x => x.VerifyAndGenerateToken(It.IsAny<RefreshTokenCommand>())).ReturnsAsync(response);

        var result = await _client.PostAsync("api/authentication/refresh-token", JsonContent.Create(request));

        result.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<AuthResultDto>(await result.Content.ReadAsStringAsync());
        data.Should().NotBeNull();
        data.Success.Should().BeTrue();
        data.RefreshToken.Should().NotBeNullOrEmpty();
        data.Token.Should().NotBeNullOrEmpty();
    }

    private SignUpCommand GenerateValidSignUpCommand()
    {
        var password = _fixture.Create<string>();
        var email = _fixture.Create<MailAddress>().Address;
        return _fixture.Build<SignUpCommand>()
            .With(x => x.Password, password)
            .With(x => x.RepeatPassword, password)
            .With(x => x.Email, email)
            .Create();
    }
}
