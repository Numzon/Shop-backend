namespace Shop.Application.SpecificationPatterns.Models;
public class SpecificationPatternDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public IReadOnlyCollection<SimpleSpecificationPatternSpecificationTypeDto> Types { get; set; } = new List<SimpleSpecificationPatternSpecificationTypeDto>();
}
