using System.Diagnostics.CodeAnalysis;

namespace Shop.Application.Common.Models;

[ExcludeFromCodeCoverage]
public sealed class ElasticsearchDto
{
    public string Uri { get; set; } = null!;
    public string DefaultIndex => WeatherForecastIndex;
    public string WeatherForecastIndex { get; set; } = null!;
}
