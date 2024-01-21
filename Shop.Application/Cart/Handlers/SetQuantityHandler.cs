using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Application.Cart.Commands.ReduceQuantity;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Handlers;
public sealed class SetQuantityHandler : IRequestHandler<SetQuantityCommand, CartDto>
{
    private readonly IDistributedCache _distributedCache;

    public SetQuantityHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<CartDto> Handle(SetQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await _distributedCache.GetRecordAsync<CartDto>(request.CartId.ToString());

        if (cart == null)
        {
            throw new InvalidOperationException("Cart hasn't been created yet.");
        }

        var product = cart.Products.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product cannot be found.");
        }

        product.Quantity = request.Quantity;

        await _distributedCache.SetCartAsync(cart);
        return cart;
    }
}
