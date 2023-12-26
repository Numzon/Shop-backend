using Shop.Domain.Common;

namespace Shop.Domain.Entities;

public sealed class ProductCategory : BaseAuditableEntity
{
    public required string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public ProductCategory? ParentCategory { get; set; }

    public Guid? SpecificationPatternId { get; set; }
    public SpecificationPattern? SpecificationPattern { get; set; }

    public IReadOnlyCollection<ProductCategory> Subcategories { get; set; } = new List<ProductCategory>();
}
