using vv_airline;
using Microsoft.EntityFrameworkCore;
using vv_airline.Database.Seeders;
using vv_airline.Models;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;
using Newtonsoft.Json;
public class Program
{
    static async Task Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();

        if (args.ElementAtOrDefault(0) == "test")
        {
            await Test(app.Services);
            return;
        }
        if (args.ElementAtOrDefault(0) == "seed")
        {
            await Seed(args, app.Services);
            return;
        }

        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
    static async Task Seed(string[] args, IServiceProvider serviceProvider)
    {
        if (args.ElementAtOrDefault(1) == "init")
        {
            await InitSeed.Seed(serviceProvider);
            return;
        }
        if (args.ElementAtOrDefault(1) == "test")
        {
            await TestSeed.Seed(serviceProvider);
            return;
        }
    }

    static async Task Test(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetService<AppDBContext>();

        // Model model = dbContext
        //     .Models
        //     .Include(m => m.Aircrafts)
        //     .First();

        // Console.WriteLine(model.Aircrafts.Count);

        Aircraft aircraft = dbContext
            .Aircrafts
            .Include(m => m.Model)
            .First();

        Console.WriteLine(aircraft.Model.Name);

        return;
    }
}

