using MediatR;
using Shop.Application.Category.Models;
using Shop.Application.Common.Models;

namespace Shop.Application.Category.Queries;
public sealed class GetMainCategoriesQuery : GetAllFiltersDto, IRequest<GetListResponseDto<CategoryListItemDto>>
{
}
