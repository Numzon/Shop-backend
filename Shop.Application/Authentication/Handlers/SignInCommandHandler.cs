using MediatR;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Authentication.Handlers;
public sealed class SignInCommandHandler : IRequestHandler<SignInCommand, AuthResultDto>
{
    private readonly IIdentityService _identityService;

    public SignInCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AuthResultDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return _identityService.SignInUser(request);
    }
}
