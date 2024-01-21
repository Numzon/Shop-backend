using Mapster;
using MediatR;
using Shop.Application.Common.Interfaces;
using Shop.Application.Products.Commands.CreateProduct;
using Shop.Application.Products.Models;
using Shop.Domain.Entities;
using Shop.Domain.Events.ProductEvents;

namespace Shop.Application.Products.Handlers;
public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, SimpleProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;

    public CreateProductHandler(IApplicationDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<SimpleProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = request.Adapt<Product>();

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        await _publisher.Publish(new ProductCreatedOrModifiedEvent { Product = product }, cancellationToken);

        return product.Adapt<SimpleProductDto>();
    }
}
