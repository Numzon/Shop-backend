using MediatR;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Queries;
public sealed class GetCartQuery : IRequest<CartDto?>
{
    public Guid CartId { get; set; }
}
    