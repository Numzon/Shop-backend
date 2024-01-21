using Shop.Application.Common.Interfaces;
using Shop.Domain.Constants;

namespace Shop.WebApi.Services;

public sealed class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _contextAccessor;

	public CurrentUserService(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public string? UserId => _contextAccessor.HttpContext?.User?.FindFirst(CustomClaimNames.Id)?.Value;
}
