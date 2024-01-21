using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;
using Shop.Domain.Entities;

namespace Shop.Application.Common.Interfaces;
public interface IIdentityService
{
    Task<AuthResultDto> SignUpUser(SignUpCommand request);
    Task<AuthResultDto> SignInUser(SignInCommand request);
    Task<AuthResultDto> GenerateTokenString(ApplicationUser user);
    Task<AuthResultDto> VerifyAndGenerateToken(RefreshTokenCommand request);
}
