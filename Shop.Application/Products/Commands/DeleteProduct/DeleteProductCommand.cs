using MediatR;

namespace Shop.Application.Products.Commands.DeleteProduct;
public sealed class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
}
