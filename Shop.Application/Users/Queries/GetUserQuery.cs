using MediatR;
using Shop.Application.Users.Models;

namespace Shop.Application.Users.Queries;
public sealed class GetUserQuery : IRequest<UserDto>
{
    public required string Id { get; set; }
}
