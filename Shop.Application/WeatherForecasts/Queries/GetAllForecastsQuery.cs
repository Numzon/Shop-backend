using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Enitites;
using Shop.Application.WeatherForecasts.Models;

namespace Shop.Application.WeatherForecasts.Queries;
public class GetAllForecastsQuery : IRequest<List<WeatherForecastDto>>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 10;
}
