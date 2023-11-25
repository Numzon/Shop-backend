using FluentValidation;
using Shop.Application.WeatherForecasts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.WeatherForecasts.Commands.CreateWeatherForecast;
public sealed class CreateWeatherForecastValidation : AbstractValidator<WeatherForecastDto>
{
	public CreateWeatherForecastValidation()
	{
		RuleFor(x => x.TemperatureC).GreaterThan(-100);
	}
}
