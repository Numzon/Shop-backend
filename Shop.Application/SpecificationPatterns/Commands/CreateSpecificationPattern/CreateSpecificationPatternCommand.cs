using MediatR;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Commands.CreateSpecificationPattern;
public sealed class CreateSpecificationPatternCommand : IRequest<SimpleSpecificationPatternDto>
{
    public required string Name { get; set; }

    public IReadOnlyCollection<SpecificationTypeIdDto> Types { get; set; } = new List<SpecificationTypeIdDto>();
}
