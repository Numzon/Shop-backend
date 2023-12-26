using Shop.Domain.Common;

namespace Shop.Domain.Entities;
public sealed class SpecificationType : BaseAuditableEntity
{
    public required string Name { get; set; }

    public Guid? ParentId { get; set; }
    public SpecificationType? Parent { get; set; }

    public IReadOnlyCollection<SpecificationType> Subtypes { get; set; } = new List<SpecificationType>();   

    public IReadOnlyCollection<SpecificationPatternSpecificationType> SpecificationPatternSpecificationTypes { get; set; } = new List<SpecificationPatternSpecificationType>(); 
}
    