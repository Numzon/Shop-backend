using MediatR;
using Shop.Application.Products.Models;

namespace Shop.Application.Products.Queries;
public sealed class GetProductQuery : IRequest<ProductDto>
{
    public Guid Id { get; set; }
}
