namespace Shop.Application.SpecificationTypes.Models;
public class SpecificationTypeDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid? ParentId { get; set; }
    public string? ParentName { get; set; }

    public IReadOnlyCollection<SpecificationTypeDto> Subtypes { get; set; } = new List<SpecificationTypeDto>();
}
