using Shop.Domain.Common;

namespace Shop.Domain.Entities;
public sealed class SpecificationPattern : BaseAuditableEntity
{
    public required string Name { get; set; }

    public IReadOnlyCollection<ProductCategory> Categories { get; init; } = new List<ProductCategory>();

    public IReadOnlyCollection<SpecificationPatternSpecificationType> SpecificationPatternSpecificationTypes { get; init; } = new List<SpecificationPatternSpecificationType>();
}
