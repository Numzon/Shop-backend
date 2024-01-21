using Shop.Domain.Common;

namespace Shop.Domain.Entities;
public class Product : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public Guid CategoryId { get; set; }
    public required ProductCategory Category { get; set; }
}
