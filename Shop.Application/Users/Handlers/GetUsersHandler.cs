using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Models;
using Shop.Application.Users.Models;
using Shop.Application.Users.Queries;
using Shop.Domain.Entities;

namespace Shop.Application.Users.Handlers;
public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, GetListResponseDto<UserListItemDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUsersHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<GetListResponseDto<UserListItemDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users
            .Where(x => string.IsNullOrEmpty(request.SearchString) || (!string.IsNullOrEmpty(x.Email) && x.Email.Contains(request.SearchString)));

        var users = await query
            .Select(x => new UserListItemDto
            {
                Id = x.Id,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed
            })
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var total = await query.CountAsync();

        return new GetListResponseDto<UserListItemDto>
        {
            Data = users,
            Meta = new MetaDto
            {
                Total = total
            }
        };
    }
}
