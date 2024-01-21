using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Queries;
public sealed class GetAllSpecificationPatternsQuery : ListFiltersDto, IRequest<GetListResponseDto<SpecificationPatternListItemDto>>
{
}
