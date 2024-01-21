namespace Shop.Application.Category.Models;
public class CategoryDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool HasSubcategories { get; set; }

    public SimpleCategoryDto? ParentCategory { get; set; }

    public CategorySimpleSpecificationPatternDto? SpecificationPattern { get; set; }

    public IReadOnlyCollection<SimpleCategoryDto> Subcategories { get; init; } = new List<SimpleCategoryDto>();
}
