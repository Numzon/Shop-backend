using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Queries;
public sealed class GetSpecificationPatternsForSelectListQuery : IRequest<IEnumerable<SimpleSpecificationPatternDto>>
{
}
