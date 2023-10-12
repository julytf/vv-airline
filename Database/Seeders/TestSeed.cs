using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using vv_airline.Models;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Database.Seeders;
public static class TestSeed
{

    static private AppDBContext? _dbContext;
    static private RoleManager<IdentityRole>? _roleManager;
    static private UserManager<User>? _userManager;
    static private IServiceProvider? _serviceProvider;
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        _serviceProvider = serviceProvider;
        _dbContext = scope.ServiceProvider.GetService<AppDBContext>();
        _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        _userManager = scope.ServiceProvider.GetService<UserManager<User>>();

        await SeatClassesSeed();
        await AircraftsSeed();
        await RoutesSeed();
        await FlightsSeed();
        await _dbContext.SaveChangesAsync();
    }


    static async Task AircraftsSeed()
    {
        Model newModel = new() { Name = "Boeing 787" };

        _dbContext.Add(newModel);
        _dbContext.SaveChanges();

        SeatMap newSeatMap = new() { ModelId = newModel.Id, NoCol = 8, NoRow = 50 };

        var aisle_cols = new[] {
            new AisleCol(){ ModelId = newModel.Id, Value = 2},
            new AisleCol(){ ModelId = newModel.Id, Value = 3},
            new AisleCol(){ ModelId = newModel.Id, Value = 6},
            new AisleCol(){ ModelId = newModel.Id, Value = 7},
        };
        var exit_rows = new[] {
            new ExitRow(){ ModelId = newModel.Id, Value = 30},
            new ExitRow(){ ModelId = newModel.Id, Value = 35},
        };

        var EconomySeatClass = _dbContext.
            SeatClasses.
            Where(sl => sl.Name == SeatEnums.Classes.Economy.ToString()).
            First();

        List<Seat> seats = new();

        for (byte x = 0; x < 8; x++)
        {
            if (x == 2 || x == 5) continue;

            for (byte y = 0; y < 50; y++)
            {
                seats.Add(new Seat()
                {
                    ModelId = newModel.Id,
                    Row = y,
                    Col = x,
                    SeatClassId = EconomySeatClass.Id,
                    Status = SeatEnums.Statuses.Available.ToString()
                });
            }
        }

        var newAircrafts = new[] {
            new Aircraft(){
                RegistrationNumber = "madangky123123",
                ModelId = newModel.Id,
                Name = "VN 19"
            },
        };

        _dbContext.Add(newSeatMap);
        _dbContext.AddRange(aisle_cols);
        _dbContext.AddRange(exit_rows);
        _dbContext.AddRange(seats);
        _dbContext.AddRange(newAircrafts);

        _dbContext.SaveChanges();
    }
    static async Task SeatClassesSeed()
    {
        foreach (var value in Enum.GetValues(typeof(SeatEnums.Classes)))
        {
            SeatClass newSeatClass = new()
            {
                Name = value.ToString()
            };
            await _dbContext.SeatClasses.AddAsync(newSeatClass);
        }

        _dbContext.SaveChanges();
    }

    static async Task FlightsSeed()
    {
        var aircraft = _dbContext.
            Aircrafts.
            // Where(ac => ac.RegistrationNumber == "madangky123123").
            First();

        var route = _dbContext.Routes.First();

        var flight = new Flight()
        {
            Name = "test flight",
            AircraftRegistrationNumber = aircraft.RegistrationNumber,
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(5),
            RouteId = route.Id
        };

        var schedule = new Schedule()
        {
            HasTransit = false,
            RouteId = flight.RouteId,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            Distance = route.Distance
        };

        _dbContext.Add(flight);

        _dbContext.SaveChanges();
    }
    static async Task RoutesSeed()
    {
        var CanThoAirport = new Airport()
        {
            Name = "Can Tho"
        };
        var HCMAirport = new Airport()
        {
            Name = "HCM"
        };

        _dbContext.Add(CanThoAirport);
        _dbContext.Add(HCMAirport);
        _dbContext.SaveChanges();

        // Console.WriteLine("here");
        // Console.WriteLine(CanThoAirport.Id);
        // Console.WriteLine(HCMAirport.Id);

        var routes = new[] {
            new Models.Data.Route() {
                DepartureAirport = CanThoAirport.Id,
                DestinationAirport = HCMAirport.Id,
                Distance = 50
            }
        };

        _dbContext.AddRange(routes);

        _dbContext.SaveChanges();
    }

    static async Task BookingsSeed()
    {

    }
}