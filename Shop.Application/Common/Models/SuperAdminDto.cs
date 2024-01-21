using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Common.Models;

[ExcludeFromCodeCoverage]
public sealed class SuperAdminDto 
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
