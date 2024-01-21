using AutoFixture.Kernel;
using Infrastructure.UnitTests.Extensions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MockQueryable.Moq;
using Moq;
using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Constants;
using Shop.Domain.Entities;
using Shop.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
namespace Infrastructure.UnitTests.Identity;

public sealed class IdentityServiceTests
{
    private readonly Mock<IOptions<JwtDto>> _jwsOptions;
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<RoleManager<IdentityRole>> _roleManager;
    private readonly Mock<TokenValidationParameters> _tokenValidationParameters;
    private readonly Mock<IApplicationDbContext> _applicationDbContext;
    private readonly Fixture _fixture;

    public IdentityServiceTests()
    {
        _jwsOptions = new Mock<IOptions<JwtDto>>();
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        _roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null!, null!, null!, null!);
        _tokenValidationParameters = new Mock<TokenValidationParameters>();
        _applicationDbContext = new Mock<IApplicationDbContext>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GenerateTokenString_UserEmailIsNullOrEmpty_ReturnServerErrorAuthResultDto()
    {
        var identityUser = _fixture.Build<ApplicationUser>().Without(x => x.Email).Create();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.GenerateTokenString(identityUser);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.ServerError);
    }

    [Fact]
    public async Task GenerateTokenString_TokenIsGeneratedRefreshTokenIsAddedToDB_ReturnFilledUpAuthResultDto()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());

        var identityUser = _fixture.Build<ApplicationUser>().Create();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.GenerateTokenString(identityUser);

        result.Should().NotBeNull();
        result.Errors.Should().BeNull();
        result.RefreshToken.Should().NotBeNull();
        result.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task VerifyAndGenerateToken_GivenTokenIsInvalid_ReturnsServerErrorObject()
    {
        var request = _fixture.Create<RefreshTokenCommand>();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.ServerError);
        result.Success.Should().BeFalse();
    }

    [Fact]

    public async Task VerifyAndGenerateToke_IncorrectAlghoritm_ReturnsInvalidTokenError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha256Signature, jtiClaimValue);
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.InvalidTokens);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_RefreshTokenDoesntExist_ReturnsInvalidTokenError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var mockSet = _fixture.CreateMany<RefreshToken>(1).AsQueryable().BuildMockDbSet();
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.InvalidTokens);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_RefreshTokenHasAlreadyBeenUsed_ReturnsInvalidTokenError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var refreshToken = _fixture.Build<RefreshToken>().With(x => x.IsUsed, true).With(x => x.Token, request.RefreshToken).Create();
        var mockSet = _fixture.CreateManyWith(5, refreshToken).AsQueryable().BuildMockDbSet();
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.InvalidTokens);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_RefreshTokenHasBeenRevoked_ReturnsInvalidTokenError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var refreshToken = _fixture.Build<RefreshToken>().With(x => x.IsUsed, false).With(x => x.IsRevoked, true).With(x => x.Token, request.RefreshToken).Create();
        var mockSet = _fixture.CreateManyWith(5, refreshToken).AsQueryable().BuildMockDbSet();
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.InvalidTokens);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_ClaimJtiIsDifferentThanJtiHoldedInRefreshToken_ReturnsInvalidTokenError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var refreshToken = _fixture.Build<RefreshToken>().With(x => x.IsUsed, false).With(x => x.IsRevoked, false).With(x => x.Token, request.RefreshToken).Create();
        var mockSet = _fixture.CreateManyWith(5, refreshToken).AsQueryable().BuildMockDbSet();
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.InvalidTokens);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_RefreshTokenHasExpired_ReturnsExpiredTokenError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var refreshToken = _fixture.Build<RefreshToken>()
            .With(x => x.IsUsed, false)
            .With(x => x.IsRevoked, false)
            .With(x => x.JwtId, jtiClaimValue)
            .With(x => x.Token, request.RefreshToken)
            .With(x => x.ExpiryDate, DateTime.Now.AddDays(-1)).Create();
        var mockSet = _fixture.CreateManyWith(5, refreshToken).AsQueryable().BuildMockDbSet();
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.ExpiredToken);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_UserCannotBeFindInDb_ReturnsServerError()
    {
        var user = _fixture.Create<IdentityUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var refreshToken = _fixture.Build<RefreshToken>()
            .With(x => x.IsUsed, false)
            .With(x => x.IsRevoked, false)
            .With(x => x.JwtId, jtiClaimValue)
            .With(x => x.Token, request.RefreshToken)
            .With(x => x.ExpiryDate, DateTime.Now.AddDays(4))
            .Create();
        var mockSet = _fixture.CreateManyWith(5, refreshToken).AsQueryable().BuildMockDbSet();
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1).And.Contain(AuthenticationErrors.ServerError);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyAndGenerateToke_SucessfullyGeneratedNewTokenAndRefreshToken_ReturnsFilledUpAuthResponseDtoObject()
    {
        var user = _fixture.Create<ApplicationUser>();
        var jwtDto = _fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create();
        var jtiClaimValue = _fixture.Create<string>();
        var tokenValidationProvider = GenerateTokenValidationParameters(jwtDto);
        var request = GenerateRefreshTokenCommand(user, jwtDto, SecurityAlgorithms.HmacSha512Signature, jtiClaimValue);
        var refreshToken = _fixture.Build<RefreshToken>()
            .With(x => x.IsUsed, false)
            .With(x => x.IsRevoked, false)
            .With(x => x.JwtId, jtiClaimValue)
            .With(x => x.Token, request.RefreshToken)
            .With(x => x.ExpiryDate, DateTime.Now.AddDays(4))
            .Create();
        var mockSet = _fixture.CreateManyWith(5, refreshToken).AsQueryable().BuildMockDbSet();
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(c => c.RefreshTokens).Returns(mockSet.Object);
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, tokenValidationProvider);

        var result = await service.VerifyAndGenerateToken(request);

        result.Should().NotBeNull();
        result.Errors.Should().BeNull();
        result.RefreshToken.Should().NotBeNull();
        result.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task SignUpUser_InvalidCommandRequest_ReturnsInvalidPayloadError()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        var command = _fixture.Build<SignUpCommand>().Without(x => x.Email).Without(x => x.Password).Create();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignUpUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1).And.Contain(AuthenticationErrors.InvalidPayload);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignUpUser_EmailIsInUse_ReturnsEmailInUserError()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(_fixture.Create<ApplicationUser>());
        var command = CreateValidSignUpCommand();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignUpUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1).And.Contain(AuthenticationErrors.EmailIsAlreadyInUse);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignUpUser_UserCreationUnsuccessfull_ReturnsEmailInUserError()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null!);
        var response = IdentityResult.Failed(_fixture.CreateMany<IdentityError>(4).ToArray());
        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(response);
        var command = CreateValidSignUpCommand();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignUpUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty().And.HaveCountGreaterThan(0);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignUpUser_UserHasBeenSuccessfullyCreated_ReturnsAuthResultDtoWithToken()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null!);
        var response = _fixture.Build<IdentityResult>().Create();
        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(response);
        var command = CreateValidSignUpCommand();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignUpUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignInUser_InvalidCommandRequest_ReturnsInvalidPayloadError()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        var command = _fixture.Build<SignInCommand>().Without(x => x.Email).Without(x => x.Password).Create();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignInUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1).And.Contain(AuthenticationErrors.InvalidPayload);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignInUser_UserDoesntExist_ReturnsIncorrectEmailOrPasswordError()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null!);
        var command = _fixture.Build<SignInCommand>().Create();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignInUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1).And.Contain(AuthenticationErrors.IncorrectEmailOrPassword);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignInUser_IncorrectPassword_ReturnsIncorrectEmailOrPasswordError()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(_fixture.Create<ApplicationUser>());
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
        var command = _fixture.Build<SignInCommand>().Create(); 
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignInUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty().And.HaveCount(1).And.Contain(AuthenticationErrors.IncorrectEmailOrPassword);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignInUser_UserSuccessfullySingedIn_ReturnsAuthResultObjectWithToken()
    {
        _jwsOptions.Setup(x => x.Value).Returns(_fixture.Build<JwtDto>().With(x => x.ExpiryTimeFrame, TimeSpan.FromHours(1)).Create());
        _applicationDbContext.Setup(x => x.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<EntityEntry<RefreshToken>>());
        _applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<int>());
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(_fixture.Create<ApplicationUser>());
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
        var command = _fixture.Build<SignInCommand>().Create();
        var service = new IdentityService(_applicationDbContext.Object, _userManager.Object, _roleManager.Object, _jwsOptions.Object, _tokenValidationParameters.Object);

        var result = await service.SignInUser(command);

        result.Should().NotBeNull();
        result.Errors.Should().BeNullOrEmpty();
        result.Success.Should().BeTrue();
        result.RefreshToken.Should().NotBeNull();
        result.Token.Should().NotBeNull();
    }

    private RefreshTokenCommand GenerateRefreshTokenCommand(IdentityUser user, JwtDto settings, string alghorithm, string jtiClaimValue)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SecretKey));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(CustomClaimNames.Id, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, jtiClaimValue),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
            }),
            Expires = DateTime.UtcNow.Add(settings.ExpiryTimeFrame),
            SigningCredentials = new SigningCredentials(securityKey, alghorithm)
        };

        var securityTokenHandler = new JwtSecurityTokenHandler();

        var token = securityTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = securityTokenHandler.WriteToken(token);

        return new RefreshTokenCommand
        {
            RefreshToken = _fixture.Create<string>(),
            Token = jwtToken,
        };
    }

    private TokenValidationParameters GenerateTokenValidationParameters(JwtDto jwtDto)
    {
        return new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtDto.SecretKey))
        };
    }

    private SignUpCommand CreateValidSignUpCommand()
    {
        var password = _fixture.Create<string>();
        var command = _fixture.Build<SignUpCommand>().With(x => x.Password, password).With(x => x.RepeatPassword, password).Create();
        return command;
    }
}
