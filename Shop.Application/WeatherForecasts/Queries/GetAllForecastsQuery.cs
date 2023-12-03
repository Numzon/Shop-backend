using MediatR;
using Shop.Application.WeatherForecasts.Models;

namespace Shop.Application.WeatherForecasts.Queries;
public class GetAllForecastsQuery : IRequest<List<WeatherForecastDto>>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 10;
}
