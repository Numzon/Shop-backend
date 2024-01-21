using Shop.Domain.Common;

namespace Shop.Domain.Events.ProductEvents;
public class ProductDeletedEvent : BaseEvent
{
    public Guid Id { get; set; }
}
