using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shop.Application.Users.Commands.ChangeEmail;
using Shop.Application.Users.Models;
using Shop.Domain.Entities;

namespace Shop.Application.Users.Handlers;
public sealed class ChangeEmailHandler : IRequestHandler<ChangeEmailCommand, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangeEmailHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            throw new InvalidOperationException("User cannot be found");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Invalid password");
        }

        //simplified for learning purpouses
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

        var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, token);

        if (!result.Succeeded) {
            throw new InvalidOperationException(string.Join("\n", result.Errors.Select(x => x.Description)));
        }

        return user.Adapt<UserDto>();
    }
}
