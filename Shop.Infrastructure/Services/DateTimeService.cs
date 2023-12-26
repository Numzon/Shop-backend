using Shop.Application.Common.Interfaces;

namespace Shop.Infrastructure.Services;
public sealed class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
