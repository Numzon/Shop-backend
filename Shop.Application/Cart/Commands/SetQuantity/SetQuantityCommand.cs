using MediatR;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Commands.ReduceQuantity;
public sealed class SetQuantityCommand : IRequest<CartDto>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
