using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.Products.Models;

namespace Shop.Application.Products.Queries;
public sealed class GetProductsQuery : ListFiltersDto, IRequest<GetListResponseDto<ProductListItemDto>>
{
}
