using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        return services;
    }
}
