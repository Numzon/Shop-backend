using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Application.Cart.Commands.AddProduct;
using Shop.Application.Cart.Models;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Cart.Handlers;
public sealed class AddProductToCartHandler : IRequestHandler<AddProductToCartCommand, CartDto>
{
    private readonly IDistributedCache _cache;
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _applicationDbContext;

    public AddProductToCartHandler(IDistributedCache cache, IDateTime dateTime, IApplicationDbContext applicationDbContext)
    {
        _cache = cache;
        _dateTime = dateTime;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<CartDto> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        await ThrowIfProductDoesntExist(request);

        if (request.CartId == Guid.Empty)
        {
            request.CartId = Guid.NewGuid();
        }

        var cart = await _cache.GetCartAsync(request.CartId);

        if (cart == null)
        {
            return await CreateNewCartWithGivenProduct(request);
        }

        var product = cart.Products.FirstOrDefault(x => x.ProductId == request.ProductId);

        if (product == null)
        {
            return await CreateNewProductAndAddItToCart(request, cart);
        }

        product.Quantity++;

        await _cache.SetCartAsync(cart);

        return cart;
    }

    private async Task ThrowIfProductDoesntExist(AddProductToCartCommand request)
    {
        var productExists = await _applicationDbContext.Products.AnyAsync(x => x.Id == request.ProductId);

        if (!productExists)
        {
            throw new InvalidOperationException("Product cannot be found!");
        }
    }

    private async Task<CartDto> CreateNewCartWithGivenProduct(AddProductToCartCommand request)
    {
        var cart = new CartDto
        {
            CartId = request.CartId,
            Created = _dateTime.Now,
            Products = new List<CartProductDto>
                {
                    new CartProductDto { ProductId = request.ProductId, Quantity = 1 }
                }
        };

        await _cache.SetCartAsync(cart);

        return cart;
    }

    private async Task<CartDto> CreateNewProductAndAddItToCart(AddProductToCartCommand request, CartDto cart)
    {
        var product = new CartProductDto
        {
            ProductId = request.ProductId,
            Quantity = 1
        };

        cart.Products.Add(product);

        await _cache.SetCartAsync(cart);

        return cart;
    }
}
