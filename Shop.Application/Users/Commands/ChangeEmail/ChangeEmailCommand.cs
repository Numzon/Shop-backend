using MediatR;
using Shop.Application.Users.Models;

namespace Shop.Application.Users.Commands.ChangeEmail;
public sealed class ChangeEmailCommand : IRequest<UserDto>
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string NewEmail { get; set; }
    public required string Password { get; set; }
}
