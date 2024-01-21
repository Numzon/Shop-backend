using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.Roles.Models;

namespace Shop.Application.Roles.Queries;
public sealed class GetRolesQuery : ListFiltersDto, IRequest<GetListResponseDto<RoleListItemDto>>
{
}
