using MediatR;
using Nest;
using Serilog;
using Shop.Domain.ElasticsearchEntities;
using Shop.Domain.Events.ProductEvents;

namespace Shop.Application.Products.EventHandlers;
public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly IElasticClient _elasticClient;

    public ProductDeletedEventHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.DeleteAsync<ESProduct>(notification.Id);

        if (!response.IsValid)
        {
            Log.Error(response.DebugInformation);
        }
    }
}
