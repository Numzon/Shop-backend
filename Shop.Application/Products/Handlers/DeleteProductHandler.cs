using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.Products.Commands.DeleteProduct;
using Shop.Domain.Events;
using Shop.Domain.Events.ProductEvents;

namespace Shop.Application.Products.Handlers;
public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public DeleteProductHandler(IApplicationDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = _context.Products.Where(x => x.Id == request.Id).FirstOrDefault();

        if (product is null)
        {
            throw new InvalidOperationException($"Cannot find product with given Id: {request.Id}");
        }

        _context.Products.Entry(product).State = EntityState.Deleted;

        await _context.SaveChangesAsync();

        await _publisher.Publish(new ProductDeletedEvent { Id = request.Id },cancellationToken);
    }
}
 