using App;
using vv_airline.Database.Seeders;
using vv_airline.Models;
using vv_airline.Models.Data;
public class Program
{
    static async Task Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();

        // await Test(app.Services);
        // return;

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


        // await Task.Delay(1);
        // var config = serviceProvider.GetService<IConfiguration>();

        // Console.WriteLine("here");
        // Console.WriteLine(config.GetConnectionString("sql"));

        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetService<AppDBContext>();


        // SeatClass seatClass = new()
        // {
        //     Name = "test"
        // };
        // Console.WriteLine("here");
        // Console.WriteLine(seatClass.Id);

        // dbContext.Add(seatClass);

        // Console.WriteLine("here");
        // Console.WriteLine(seatClass.Id);

        Airport airport = new() { Name = "test" };

        Console.WriteLine("here");
        Console.WriteLine(airport.Id);

        dbContext.Add(airport);

        Console.WriteLine("here");
        Console.WriteLine(airport.Id);
        ;
        dbContext.SaveChanges();

        Console.WriteLine("here");
        Console.WriteLine(airport.Id);

        // await dbContext.CreateDatabase();

        // Model model = new Model
        // {
        //     Id = 1,
        //     Name = "model 1"
        // };

        // appDBContext.Models.Add(model);
        // await appDBContext.SaveChangesAsync();

        // User user = new User(){};

        // Console.WriteLine(user.Id);
    }
}

