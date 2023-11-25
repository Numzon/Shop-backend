using Mapster;
using Shop.Application.WeatherForecasts.Models;
using Shop.Domain.Enitites;

namespace Shop.Application.Common.Mappings;

public static class MapsterConfig
{
    public static TypeAdapterConfig GetGlobalSettingsConfigurationForMapster()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<WeatherForecast, WeatherForecastDto>()
            .Map(dest => dest.Date, src => src.Date);
       
        return config;
    }
}
