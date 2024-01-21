using Shop.Application.Category.Models;

namespace Shop.Application.Products.Models;
public class ProductDetailsDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<SimpleCategoryDto> CategoriesPath { get; set; } = new List<SimpleCategoryDto>();
}
