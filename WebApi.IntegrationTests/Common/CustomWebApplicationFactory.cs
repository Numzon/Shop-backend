using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shop.WebApi;
using Shop.WebApi.Program.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace WebApi.IntegrationTests.Common;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ISender> Sender { get; }

    public CustomWebApplicationFactory()
    {
        Sender = new Mock<ISender>();   
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services => services.AddSingleton(Sender.Object));
    }
}
