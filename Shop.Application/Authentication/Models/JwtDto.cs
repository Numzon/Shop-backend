using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Authentication.Models;

[ExcludeFromCodeCoverage]
public sealed class JwtDto
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public TimeSpan ExpiryTimeFrame { get; set; }
}
