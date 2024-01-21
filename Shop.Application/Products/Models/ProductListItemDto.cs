using Shop.Application.Category.Models;

namespace Shop.Application.Products.Models;
public class ProductListItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required SimpleCategoryDto Category { get; set; }
}
