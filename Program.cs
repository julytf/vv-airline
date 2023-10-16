using vv_airline;
using Microsoft.EntityFrameworkCore;
using vv_airline.Database.Seeders;
using vv_airline.Models;
using vv_airline.Models.Data;
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

        // Console.WriteLine(
        //     dbContext
        //         .Districts
        //         .Where(d => d.Province.Code == "01")
        //         .ToQueryString()
        // );

        // dbContext.Add(new Airport()
        // {
        //     Name = "test"
        // });

        // dbContext.SaveChanges();

        // var flight = dbContext
        //     .Flights
        //     // .Include(f => f.Schedules)
        //     .First();
        // var schedule = flight.Schedules.First();
        // Console.WriteLine(flight.Schedules.First().Distance);

        // var district = dbContext.Districts.First();
        // Console.WriteLine("-------------------");
        // Console.WriteLine(district.FullName);
        // Console.WriteLine("-------------------");
        // var province = district.Province;
        // Console.WriteLine("-------------------");
        // Console.WriteLine(province.FullName);
        // Console.WriteLine("-------------------");


        var query = dbContext
            .Provinces
            .Where(p => p.Code == "01")
            .Include(p => p.Districts);
        Console.WriteLine(query.ToQueryString());

        // var province = query
        //     .First(p => p.Code == "01");

        // List<District> districts = province
        //     .Districts
        //     .ToList();

        // Console.WriteLine("-----");
        // Console.WriteLine(province.FullName);
        // Console.WriteLine("-----");
        // Console.WriteLine(districts.Count());
    }
}

