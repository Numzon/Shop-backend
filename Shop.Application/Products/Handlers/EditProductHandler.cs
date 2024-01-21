using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.Products.Commands.EditProduct;
using Shop.Application.Products.Models;
using Shop.Domain.Entities;
using Shop.Domain.Events;
using Shop.Domain.Events.ProductEvents;

namespace Shop.Application.Products.Handlers;
public sealed class EditProductHandler : IRequestHandler<EditProductCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public EditProductHandler(IApplicationDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<ProductDto> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (product is null)
        {
            throw new InvalidOperationException("Product with given Id cannot be found");
        }

        request.Adapt(product);

        _context.Products.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        await _publisher.Publish(new ProductCreatedOrModifiedEvent { Product = product }, cancellationToken);

        return product.Adapt<ProductDto>();
    }
}
