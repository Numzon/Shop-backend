using Shop.Domain.Common;

namespace Shop.Domain.Entities;
public class SpecificationPatternSpecificationType : BaseAuditableEntity
{
    public Guid SpecificationPatternId { get; set; }
    public SpecificationPattern? SpecificationPattern { get; set; }

    public Guid SpecificationTypeId { get; set; }
    public SpecificationType? SpecificationType { get; set; }
}
