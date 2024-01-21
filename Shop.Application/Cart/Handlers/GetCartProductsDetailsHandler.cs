using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Application.Cart.Models;
using Shop.Application.Cart.Queries;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Cart.Handlers;
public class GetCartProductsDetailsHandler : IRequestHandler<GetCartProductsDetailsQuery, GetCartProductDetailsResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IApplicationDbContext _context;

    public GetCartProductsDetailsHandler(IDistributedCache cache, IApplicationDbContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<GetCartProductDetailsResponse> Handle(GetCartProductsDetailsQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cache.GetCartAsync(request.Id);

        if (cart == null)
        {
            return new GetCartProductDetailsResponse();
        }

        var productsIds = cart.Products.Select(x => x.ProductId).ToList();

        if (!productsIds.Any())
        {
            return new GetCartProductDetailsResponse();
        }

        var products = await _context.Products.Where(x => productsIds.Contains(x.Id)).ToListAsync();

        if (products == null || products.Count == 0)
        {
            throw new InvalidOperationException("Cannot find given products");
        }

        var cartProductsDetails = new List<CartProductDetailsDto>();
        foreach (var cartProduct in cart.Products)
        {
            var product = products.FirstOrDefault(x => x.Id == cartProduct.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product cannot be found, Id: {cartProduct.ProductId}");
            }

            cartProductsDetails.Add(new CartProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = cartProduct.Quantity
            });
        }

        return new GetCartProductDetailsResponse
        {
            CartProducts = cartProductsDetails
        };
    }
}
