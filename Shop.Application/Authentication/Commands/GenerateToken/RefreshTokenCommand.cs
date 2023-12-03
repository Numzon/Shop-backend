using MediatR;
using Shop.Application.Authentication.Models;
using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Authentication.Commands.GenerateToken;

[ExcludeFromCodeCoverage]
public sealed class RefreshTokenCommand : IRequest<AuthResultDto>
{
    public string? Token { get; set; } 
    public string? RefreshToken { get; set; } 
}
