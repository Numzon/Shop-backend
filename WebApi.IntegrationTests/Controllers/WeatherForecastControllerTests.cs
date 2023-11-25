using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Shop.Application.WeatherForecasts.Models;
using Shop.Application.WeatherForecasts.Queries;
using System.Net;
using WebApi.IntegrationTests.Common;
using Xunit;

namespace WebApi.IntegrationTests.Controllers;
public sealed class WeatherForecastControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Fixture _fixture;

    public WeatherForecastControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
        _fixture = new Fixture();
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }

    [Fact]
    public async Task GetAsync_TwoWeatherForecastsInDatabase_ReturnsTwoDtoObjects()
    {
        var expectedResult = _fixture.Build<WeatherForecastDto>().CreateMany(2).ToList();
        _factory.Sender.Setup(x => x.Send(It.IsAny<GetAllForecastsQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(expectedResult)).Verifiable();

        var response = await _client.GetAsync("api/weatherForecasts");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        var data = JsonConvert.DeserializeObject<List<WeatherForecastDto>>(await response.Content.ReadAsStringAsync());
        data.Should().NotBeNull().And.HaveCount(expectedResult.Count);
        _factory.Sender.VerifyAll();
    }

}
