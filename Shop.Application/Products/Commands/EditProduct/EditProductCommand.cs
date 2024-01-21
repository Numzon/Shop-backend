using MediatR;
using Shop.Application.Products.Models;

namespace Shop.Application.Products.Commands.EditProduct;
public sealed class EditProductCommand : IRequest<ProductDto>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
}
