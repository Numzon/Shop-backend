using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Application.Cart.Models;
using Shop.Application.Cart.Queries;

namespace Shop.Application.Cart.Handlers;
public sealed class GetCartHandler : IRequestHandler<GetCartQuery, CartDto?>
{
    private readonly IDistributedCache _cache;

    public GetCartHandler(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<CartDto?> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        return await _cache.GetCartAsync(request.CartId);
    }
}
