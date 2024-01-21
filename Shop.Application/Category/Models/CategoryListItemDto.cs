namespace Shop.Application.Category.Models;
public class CategoryListItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int SubcategoriesCount { get; set; }
}
