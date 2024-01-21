using MediatR;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Queries;
public sealed class GetSpecificationPatternQuery : IRequest<SpecificationPatternDto>
{
    public Guid Id { get; set; }
}
