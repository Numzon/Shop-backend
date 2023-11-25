using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.WeatherForecasts.Models;
using Shop.Application.WeatherForecasts.Queries;

namespace Shop.WebApi.Controllers;
[ApiController]
[Route("api/weatherForecasts")]
public class WeatherForecastController : ControllerBase
{
    private readonly ISender _sender;

    public WeatherForecastController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<List<WeatherForecastDto>>> Get([FromQuery] GetAllForecastsQuery query)
    {
        return await _sender.Send(query);
    }
}
