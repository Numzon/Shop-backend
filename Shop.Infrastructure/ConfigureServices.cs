using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Domain.Enitites;
using Shop.Infrastructure.Persistance;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringDto = configuration.GetRequiredSection("Database").Get<DatabaseDto>();
        var connectionString = BuildConnectionString(connectionStringDto);

        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddElasticSearch(configuration);

        services.AddRedis(configuration);

        services.SetUpDependencyInjection();

        services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    private static string BuildConnectionString(DatabaseDto? databaseDto)
    {
        if (databaseDto is null)
        {
            throw new InvalidOperationException("Database section cannot be found. Please check your secret manager.");
        }

        var sqlStringBuilder = new SqlConnectionStringBuilder();

        sqlStringBuilder.UserID = databaseDto.UserId;
        sqlStringBuilder.Password = databaseDto.Password;
        sqlStringBuilder.TrustServerCertificate = databaseDto.TrustServerCertificate;
        sqlStringBuilder.DataSource = databaseDto.DataSource;
        sqlStringBuilder.InitialCatalog = databaseDto.InitialCatalog;

        return sqlStringBuilder.ConnectionString;
    }

    private static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var elasticsearchDto = configuration.GetRequiredSection("Elasticsearch").Get<ElasticsearchDto>();

        if (elasticsearchDto is null)
        {
            throw new InvalidOperationException("Elasticsearch section cannot be found. Please check your secret manager.");
        }

        var settings = new ConnectionSettings(new Uri(elasticsearchDto.Uri))
            .PrettyJson()
            .DefaultIndex(elasticsearchDto.DefaultIndex);

        settings = AddDefaultMappings(settings, elasticsearchDto);

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);

        CreateIndexes(client, elasticsearchDto);

        return services;
    }

    private static ConnectionSettings AddDefaultMappings(ConnectionSettings settings, ElasticsearchDto elasticsearchDto)
    {
        settings.DefaultMappingFor<WeatherForecast>(s => s.IndexName(elasticsearchDto.WeatherForecastIndex)
            .Ignore(z => z.TemperatureF)
            );

        return settings;
    }
    private static void CreateIndexes(IElasticClient client, ElasticsearchDto elasticsearchDto)
    {
        client.Indices.Create(elasticsearchDto.WeatherForecastIndex, i => i.Map<WeatherForecast>(x => x.AutoMap()));
    }

    private static IServiceCollection SetUpDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisDto = configuration.GetRequiredSection("Redis").Get<RedisDto>();

        if (redisDto is null)
        {
            throw new InvalidOperationException("Redis section cannot be found. Please check your secret manager.");
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisDto.Uri;
            options.InstanceName = "Shop_";
        });

        return services;
    }
}
