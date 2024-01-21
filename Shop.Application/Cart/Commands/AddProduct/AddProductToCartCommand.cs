using MediatR;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Commands.AddProduct;
public sealed class AddProductToCartCommand : IRequest<CartDto>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
}
