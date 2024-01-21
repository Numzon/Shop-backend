using MediatR;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Commands.RemoveProduct;
public sealed class RemoveProductCommand : IRequest<CartDto>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
}
