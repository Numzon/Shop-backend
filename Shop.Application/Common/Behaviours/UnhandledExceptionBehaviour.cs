using MediatR;
using Serilog;

namespace Shop.Application.Common.Behaviours;
public sealed class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
		try
		{
			return await next();
		}
		catch (Exception ex)
		{
			var requestName = typeof(TRequest).Name;
			Log.Error($"Unhandled exception for request {requestName} {request}", ex);
			throw;
		}
    }
}
