using MediatR;
using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Authentication.Handlers;
public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IIdentityService _identityService;

    public RefreshTokenCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.VerifyAndGenerateToken(request);
    }
}
