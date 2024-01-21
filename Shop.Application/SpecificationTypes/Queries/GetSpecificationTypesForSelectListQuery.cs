using MediatR;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationTypes.Queries;
public sealed class GetSpecificationTypesForSelectListQuery : IRequest<IEnumerable<SimpleSpecificationTypeDto>>
{
}
