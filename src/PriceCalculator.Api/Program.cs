namespace PriceCalculator.Api;

public class Program
{
    public static void Main()
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());
        
        builder.Build().Run();
    }
}