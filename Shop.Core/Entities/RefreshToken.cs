namespace Shop.Domain.Entities;
public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string JwtId { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Token { get; set; } = string.Empty;
}
