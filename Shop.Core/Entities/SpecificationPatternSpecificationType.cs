using Shop.Domain.Common;

namespace Shop.Domain.Entities;
public class SpecificationPatternSpecificationType : BaseAuditableEntity
{
    public Guid SpecificationPatternId { get; set; }
    public required SpecificationPattern SpecificationPattern { get; set; }

    public Guid SpecificationTypeId { get; set; }
    public required SpecificationType SpecificationType { get; set; }
}
