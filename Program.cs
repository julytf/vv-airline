using App;
using vv_airline.Database.Seeders;
using vv_airline.Models;
using vv_airline.Models.Data;
public class Program
{
    static async Task Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();

        if (args.Length > 0 && args[0] == "seed")
        {
            await InitSeed.Seed(app.Services);
            return;
        }

        await Test(app.Services);
        
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

    static async Task Test(IServiceProvider serviceProvider)
    {

        // var config = serviceProvider.GetService<IConfiguration>();

        // Console.WriteLine("here");
        // Console.WriteLine(config.GetConnectionString("sql"));

        // using var scope = serviceProvider.CreateScope();

        // var context = scope.ServiceProvider.GetService<AppDBContext>();

        // await context.CreateDatabase();

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

