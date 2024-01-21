namespace Shop.Application.SpecificationTypes.Models;
public class SpecificationTypeDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public SimpleSpecificationTypeDto? Parent { get; set; }

    public IReadOnlyCollection<SimpleSpecificationTypeDto> Subtypes { get; set; } = new List<SimpleSpecificationTypeDto>();
}
