using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Application.Cart.Commands.RemoveProduct;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Handlers;
public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, CartDto>
{
    private readonly IDistributedCache _distributedCache;

    public RemoveProductHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<CartDto> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        var cart = await _distributedCache.GetCartAsync(request.CartId);

        if (cart == null)
        {
            throw new InvalidOperationException("Cart cannot be found.");
        }

        var product = cart.Products.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product cannot be found.");
        }

        cart.Products.Remove(product);

        await _distributedCache.SetCartAsync(cart);

        return cart;
    }
}
