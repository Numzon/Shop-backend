using MediatR;
using Shop.Application.Authentication.Models;
using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Authentication.Commands.SignIn;

[ExcludeFromCodeCoverage]
public sealed class SignInCommand : IRequest<AuthResultDto>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
