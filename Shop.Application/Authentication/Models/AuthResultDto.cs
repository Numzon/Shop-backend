using Shop.Application.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Authentication.Models;

[ExcludeFromCodeCoverage]
public sealed class AuthResultDto : CreateUpdateSimpleResponseDto
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
