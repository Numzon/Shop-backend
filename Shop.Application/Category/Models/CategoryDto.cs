namespace Shop.Application.Category.Models;
public class CategoryDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }

    public IReadOnlyCollection<CategoryDto> Subcategories { get; set; } = new List<CategoryDto>();
}
