using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace PriceCalculator.Domain.IntegrationTests;

public sealed class AppFixture : WebApplicationFactory<Api.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddControllers()
                .AddControllersAsServices();
        });
        base.ConfigureWebHost(builder);
    }
}