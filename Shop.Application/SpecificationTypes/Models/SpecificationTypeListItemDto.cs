namespace Shop.Application.SpecificationTypes.Models;
public class SpecificationTypeListItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int SubspecificationTypeCount { get; set; }
}
