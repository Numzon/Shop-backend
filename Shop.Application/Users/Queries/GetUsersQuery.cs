using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.Users.Models;

namespace Shop.Application.Users.Queries;
public sealed class GetUsersQuery : ListFiltersDto, IRequest<GetListResponseDto<UserListItemDto>>
{
}
