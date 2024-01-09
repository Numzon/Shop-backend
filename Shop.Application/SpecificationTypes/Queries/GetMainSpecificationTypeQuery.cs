using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.Category.Queries;
public sealed class GetAllMainSpecificationTypesQuery : GetAllFiltersDto, IRequest<GetListResponseDto<SpecificationTypeListItemDto>>
{
}
