using PriceCalculator.Dal.Extensions;

namespace PriceCalculator.Api;

public class Program
{
    public static void Main()
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());

        var app = builder.Build();

        app.MigrateUp();
        app.Run();
    }
}