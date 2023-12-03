using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Authentication.Models;

[ExcludeFromCodeCoverage]
public sealed class AuthResultDto
{
    public string Token { get; set; } = null!;
    public bool Success { get; set; }
    public string RefreshToken { get; set; } = null!;
    public IEnumerable<string> Errors { get; set; } = null!;
}
