namespace Shop.Application.Users.Models;
public class UserDto
{
    public required string Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
}
