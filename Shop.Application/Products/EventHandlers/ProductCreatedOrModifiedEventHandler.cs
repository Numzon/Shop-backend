using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nest;
using Serilog;
using Shop.Application.Common.Interfaces;
using Shop.Domain.ElasticsearchEntities;
using Shop.Domain.Entities;
using Shop.Domain.Events.ProductEvents;

namespace Shop.Application.Products.EventHandlers;
public sealed class ProductCreatedOrModifiedEventHandler : INotificationHandler<ProductCreatedOrModifiedEvent>
{
    private readonly IElasticClient _elasticClient;
    private readonly IApplicationDbContext _context;

    public ProductCreatedOrModifiedEventHandler(IElasticClient elasticClient, IApplicationDbContext context)
    {
        _elasticClient = elasticClient;
        _context = context;
    }

    public async Task Handle(ProductCreatedOrModifiedEvent notification, CancellationToken cancellationToken)
    {

        if (notification.Product.Category != null)
        {
            await AddProductToElasticsearch(notification.Product);
            return;
        }

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == notification.Product.CategoryId);

        if (category == null)
        {
            Log.Error($"Category cannot be found! Id: {notification.Product.CategoryId}");
            return;
        }

        notification.Product.Category = category;

        await AddProductToElasticsearch(notification.Product);
    }

    private async Task AddProductToElasticsearch(Product product)
    {
        var esProduct = product.Adapt<ESProduct>();

        var response = await _elasticClient.IndexDocumentAsync(esProduct);

        if (!response.IsValid)
        {
            Log.Error(response.DebugInformation);
        }
    }
}
