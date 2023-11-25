namespace Shop.Application.Common.Models;
public sealed class ElasticsearchDto
{
    public string Uri { get; set; } = null!;
    public string DefaultIndex => WeatherForecastIndex;
    public string WeatherForecastIndex { get; set; } = null!;
}
