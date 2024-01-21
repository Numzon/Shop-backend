using MediatR;
using Shop.Application.Roles.Models;

namespace Shop.Application.Roles.Commands;
public sealed class CreateRoleCommand : IRequest<SimpleRoleDto>
{
    public required string Name { get; set; }
}
