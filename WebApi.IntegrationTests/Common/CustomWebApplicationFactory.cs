using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Common.Interfaces;
using Shop.WebApi.Program.Main;

namespace WebApi.IntegrationTests.Common;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IIdentityService> IdentityService { get; }

    public CustomWebApplicationFactory()
    {
        IdentityService = new Mock<IIdentityService>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services => services.AddSingleton(IdentityService.Object));
    }
}
