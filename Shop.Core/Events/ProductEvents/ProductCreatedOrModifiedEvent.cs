using Shop.Domain.Common;
using Shop.Domain.Entities;

namespace Shop.Domain.Events.ProductEvents;
public class ProductCreatedOrModifiedEvent : BaseEvent
{
    public required Product Product { get; set; }
}
