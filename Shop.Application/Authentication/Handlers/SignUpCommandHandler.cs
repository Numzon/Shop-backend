using MediatR;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Authentication.Handlers;
public sealed class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthResultDto>
{
    private readonly IIdentityService _identityService;

    public SignUpCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResultDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.SignUpUser(request);
    }
}
    