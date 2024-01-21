using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Models;
using Shop.Application.Roles.Models;
using Shop.Application.Roles.Queries;

namespace Shop.Application.Roles.Handlers;
public sealed class GetRolesHandler : IRequestHandler<GetRolesQuery, GetListResponseDto<RoleListItemDto>>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public GetRolesHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<GetListResponseDto<RoleListItemDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var query = _roleManager.Roles
            .Where(x => string.IsNullOrEmpty(request.SearchString)
                || (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(request.SearchString))
                || (!string.IsNullOrEmpty(x.NormalizedName) && x.NormalizedName.Contains(request.SearchString)));

        var roles = await query
            .Select(x => new RoleListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName
            })
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var total = await query.CountAsync();

        return new GetListResponseDto<RoleListItemDto>
        {
            Data = roles,
            Meta = new MetaDto
            {
                Total = total
            }
        };
    }
}
