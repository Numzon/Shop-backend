using MediatR;
using Shop.Application.Products.Models;

namespace Shop.Application.Products.Queries;
public sealed class GetProductDetailsQuery : IRequest<ProductDetailsDto>
{
    public required Guid Id { get; set; }
}
