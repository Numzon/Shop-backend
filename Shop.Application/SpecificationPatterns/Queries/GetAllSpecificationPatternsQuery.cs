using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Queries;
public sealed class GetAllSpecificationPatternsQuery : GetAllFiltersDto, IRequest<GetListResponseDto<SpecificationPatternListItemDto>>
{
}
