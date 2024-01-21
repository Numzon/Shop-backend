using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shop.Application.Roles.Commands;
using Shop.Application.Roles.Models;

namespace Shop.Application.Roles.Handlers;
public sealed class CreateRoleHandler : IRequestHandler<CreateRoleCommand, SimpleRoleDto>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public CreateRoleHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<SimpleRoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var identityRole = request.Adapt<IdentityRole>();
        var result = await _roleManager.CreateAsync(identityRole);

        if (!result.Succeeded)
        {

            throw new InvalidOperationException(string.Join("\n", result.Errors.Select(x => x.Description)));
        }

        return identityRole.Adapt<SimpleRoleDto>();

    }
}
