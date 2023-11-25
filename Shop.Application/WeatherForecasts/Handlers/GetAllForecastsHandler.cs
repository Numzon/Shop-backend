using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Nest;
using Shop.Application.Common.Interfaces;
using Shop.Application.WeatherForecasts.Models;
using Shop.Application.WeatherForecasts.Queries;
using Shop.Domain.Enitites;

namespace Shop.Application.WeatherForecasts.Handlers;
public class GetAllForecastsHandler : IRequestHandler<GetAllForecastsQuery, List<WeatherForecastDto>>
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IApplicationDbContext _context;
    private readonly IDistributedCache _distributedCache;
    private readonly IElasticClient _elasticClient;

    public GetAllForecastsHandler(IApplicationDbContext context, IDistributedCache distributedCache, IElasticClient elasticClient)
    {
        _context = context;
        _distributedCache = distributedCache;
        _elasticClient = elasticClient;
        //_distributedCache.SetRecordAsync()
    }

    public async Task<List<WeatherForecastDto>> Handle(GetAllForecastsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(Enumerable.Range(1, 15)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).Select(x => new WeatherForecastDto
                {
                    Date = x.Date,
                    Summary = x.Summary,
                    TemperatureC = x.TemperatureC,
                    TemperatureF = x.TemperatureF
                }).Skip(request.PageIndex * request.PageSize).Take(request.PageSize)
       .ToList());
    }
}
