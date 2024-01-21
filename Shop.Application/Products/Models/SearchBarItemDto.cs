namespace Shop.Application.Products.Models;
public class SearchBarItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public bool IsCategory { get; set; } = false;
}
