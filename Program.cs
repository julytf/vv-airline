using vv_airline;
using Microsoft.EntityFrameworkCore;
using vv_airline.Database.Seeders;
using vv_airline.Models;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;
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

        var schedule = dbContext
                    .Schedules
                    .Include(s => s.Flights)
                    .First();

        var flight = schedule.Flights[0];
        Console.WriteLine(flight.Id);

        // var aircraft = flight.Aircraft;
        // Console.WriteLine(aircraft.RegistrationNumber);

            

        return;
    }
}

