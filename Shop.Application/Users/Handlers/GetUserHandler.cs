using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Users.Models;
using Shop.Application.Users.Queries;
using Shop.Domain.Entities;

namespace Shop.Application.Users.Handlers;
public sealed class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager
            .Users
            .Where(x => x.Id == request.Id)
            .Select(x => new UserDto
            {
                Id = x.Id,
                Email= x.Email,
                UserName = x.UserName
            }).FirstOrDefaultAsync();

        if (user is null)
        {
            throw new InvalidOperationException($"User cannot be found!");
        }

        return user;
    }
}
