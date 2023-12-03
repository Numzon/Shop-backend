using Microsoft.AspNetCore.Identity;
using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;

namespace Shop.Application.Common.Interfaces;
public interface IIdentityService
{
    Task<AuthResultDto> SignUpUser(SignUpCommand request);
    Task<AuthResultDto> SignInUser(SignInCommand request);
    Task<AuthResultDto> GenerateTokenString(IdentityUser user);
    Task<AuthResultDto> VerifyAndGenerateToken(RefreshTokenCommand request);
}
