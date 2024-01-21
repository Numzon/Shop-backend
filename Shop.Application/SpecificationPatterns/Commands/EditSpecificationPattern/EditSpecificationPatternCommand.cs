using MediatR;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Commands.EditSpecificationPattern;
public sealed class EditSpecificationPatternCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public IReadOnlyCollection<SimpleSpecificationPatternSpecificationTypeDto> Types { get; set; } = new List<SimpleSpecificationPatternSpecificationTypeDto>();
}
