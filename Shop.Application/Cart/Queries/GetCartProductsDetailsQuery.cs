using MediatR;
using Shop.Application.Cart.Models;

namespace Shop.Application.Cart.Queries;
public sealed class GetCartProductsDetailsQuery : IRequest<GetCartProductDetailsResponse>
{
    public Guid Id { get; set; }
}
