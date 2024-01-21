namespace Shop.Application.Users.Models;
public class UserListItemDto
{
    public required string Id { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
}
