using MediatR;
using Shop.Application.Authentication.Models;
using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Authentication.Commands.SignUp;

[ExcludeFromCodeCoverage]
public sealed class SignUpCommand : IRequest<AuthResultDto>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? RepeatPassword { get; set; }
}
