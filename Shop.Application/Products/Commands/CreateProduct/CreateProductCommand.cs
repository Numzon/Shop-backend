using MediatR;
using Shop.Application.Products.Models;

namespace Shop.Application.Products.Commands.CreateProduct;
public sealed class CreateProductCommand : IRequest<SimpleProductDto>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
}
