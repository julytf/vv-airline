using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using vv_airline.Models;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Database.Seeders;
public static class TestSeed
{

    static private AppDBContext? _appDBContext;
    static private RoleManager<IdentityRole>? _roleManager;
    static private UserManager<User>? _userManager;
    static private IServiceProvider? _serviceProvider;
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        _serviceProvider = serviceProvider;
        _appDBContext = scope.ServiceProvider.GetService<AppDBContext>();
        _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        _userManager = scope.ServiceProvider.GetService<UserManager<User>>();

        await SeatClassesSeed();
        await AircraftsSeed();
        await RoutesSeed();
        await FlightsSeed();
        await _appDBContext.SaveChangesAsync();
    }


    static async Task AircraftsSeed()
    {
        Model boing787Model = new() { Name = "Boeing 787" };

        _appDBContext.Add(boing787Model);
        _appDBContext.SaveChanges();

        SeatMap boing787SeatMap = new() { ModelId = boing787Model.Id, NoCol = 8, NoRow = 50 };

        var aisle_cols = new[] {
            // new AisleCol(){ SeatMap = boing787SeatMap, Value = 2},
            new AisleCol(){ SeatMap = boing787SeatMap, Value = 'C'},
            new AisleCol(){ SeatMap = boing787SeatMap, Value = 'F'},
            // new AisleCol(){ SeatMap = boing787SeatMap, Value = 7},
        };
        var exit_rows = new[] {
            new ExitRow(){ SeatMap = boing787SeatMap, Value = 30},
            new ExitRow(){ SeatMap = boing787SeatMap, Value = 35},
        };

        var economySeatClass = _appDBContext.
            SeatClasses.
            Where(sl => sl.Name == SeatEnums.Classes.Economy.ToString()).
            Single();

        List<Seat> boing787Seats = new();

        for (char x = 'A'; x < 'H' + 1; x++)
        {
            if (x == 'C' || x == 'F') continue;

            for (byte y = 1; y <= 50; y++)
            {
                boing787Seats.Add(new Seat()
                {
                    SeatMap = boing787SeatMap,
                    Row = y,
                    Col = x,
                    SeatClass = economySeatClass,
                    Status = SeatEnums.Statuses.Available
                });
            }
        }

        var newAircrafts = new[] {
            new Aircraft(){
                RegistrationNumber = "boing787_12345",
                Model = boing787Model,
                Name = "VN 19"
            },
            new Aircraft(){
                RegistrationNumber = "boing787_54321",
                Model = boing787Model,
                Name = "VN 23"
            },
        };

        _appDBContext.Add(boing787SeatMap);
        _appDBContext.AddRange(aisle_cols);
        _appDBContext.AddRange(exit_rows);
        _appDBContext.AddRange(boing787Seats);
        _appDBContext.AddRange(newAircrafts);

        _appDBContext.SaveChanges();
    }
    static async Task SeatClassesSeed()
    {
        foreach (var value in Enum.GetValues(typeof(SeatEnums.Classes)))
        {
            SeatClass newSeatClass = new()
            {
                Name = value.ToString()
            };
            await _appDBContext.SeatClasses.AddAsync(newSeatClass);
        }

        _appDBContext.SaveChanges();
    }

    static async Task FlightsSeed()
    {
        var economySeatClass = _appDBContext.
            SeatClasses.
            Where(sl => sl.Name == SeatEnums.Classes.Economy.ToString()).
            Single();
        var businessSeatClass = _appDBContext.
            SeatClasses.
            Where(sl => sl.Name == SeatEnums.Classes.Business.ToString()).
            Single();

        var vn19Aircraft = _appDBContext.
            Aircrafts.
            Where(ac => ac.RegistrationNumber == "Boing787_12345").
            Single();

        var haNoiAirport = _appDBContext
            .Airports
            .Single(ap => ap.NameEn == "Ha Noi");
        var hoChiMinhAirport = _appDBContext
            .Airports
            .Single(ap => ap.NameEn == "Ho Chi Minh");
        var daNangAirport = _appDBContext
            .Airports
            .Single(ap => ap.NameEn == "Da Nang");

        // Console.WriteLine(haNoiAirport.Name);
        // Console.WriteLine(haNoiAirport.Id);
        // Console.WriteLine(hoChiMinhAirport.Name);
        // Console.WriteLine(hoChiMinhAirport.Id);

        var haNoiToHCMFlightRoute = _appDBContext
            .FlightRoutes
            .Single(fr =>
                fr.DepartureAirport == haNoiAirport
                && fr.DestinationAirport == hoChiMinhAirport
            );
        // Console.WriteLine(haNoiToHCMFlightRoute.Id);
        var hCMToDaNangFlightRoute = _appDBContext
            .FlightRoutes
            .Single(fr =>
                fr.DepartureAirport == hoChiMinhAirport
                && fr.DestinationAirport == daNangAirport
            );

        var daNangToHaNoiFlightRoute = _appDBContext
            .FlightRoutes
            .Single(fr =>
                fr.DepartureAirport == daNangAirport
                && fr.DestinationAirport == haNoiAirport
            );

        var hCMToHaNoiFlightRoute = _appDBContext
            .FlightRoutes
            .Single(fr =>
                fr.DepartureAirport == hoChiMinhAirport
                && fr.DestinationAirport == haNoiAirport
            );


        List<DefaultPrice> defaultPrices = new(){
            new (){
                FlightRoute = haNoiToHCMFlightRoute,
                SeatClass = economySeatClass,
                Value = 1_000_000
            },
            new (){
                FlightRoute = haNoiToHCMFlightRoute,
                SeatClass = businessSeatClass,
                Value = 2_000_000
            },
            new (){
                FlightRoute = hCMToDaNangFlightRoute,
                SeatClass = economySeatClass,
                Value = 520_000
            },
            new (){
                FlightRoute = hCMToDaNangFlightRoute,
                SeatClass = businessSeatClass,
                Value = 1_100_000
            },
            new (){
                FlightRoute = daNangToHaNoiFlightRoute,
                SeatClass = economySeatClass,
                Value = 520_000
            },
            new (){
                FlightRoute = daNangToHaNoiFlightRoute,
                SeatClass = businessSeatClass,
                Value = 1_100_000
            },
            new (){
                FlightRoute = hCMToHaNoiFlightRoute,
                SeatClass = economySeatClass,
                Value = 1_000_000
            },
            new (){
                FlightRoute = hCMToHaNoiFlightRoute,
                SeatClass = businessSeatClass,
                Value = 2_000_000
            },
        };
        _appDBContext.AddRange(defaultPrices);
        _appDBContext.SaveChanges();


        var haNoiToHCMFlight = new Flight()
        {
            Name = "HN to HCM flight",
            Aircraft = vn19Aircraft,
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(5),
            FlightRoute = haNoiToHCMFlightRoute
        };



        var hCMToDaNangFlight = new Flight()
        {
            Name = "HN to HCM flight",
            Aircraft = vn19Aircraft,
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(5),
            FlightRoute = hCMToDaNangFlightRoute
        };

        var daNangToHaNoiFlight = new Flight()
        {
            Name = "HN to HCM flight",
            Aircraft = vn19Aircraft,
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(5),
            FlightRoute = daNangToHaNoiFlightRoute
        };

        _appDBContext.Add(haNoiToHCMFlight);
        _appDBContext.Add(hCMToDaNangFlight);
        _appDBContext.Add(daNangToHaNoiFlight);

        _appDBContext.SaveChanges();

        var haNoiToHCMFSchedule = new Schedule()
        {
            HasTransit = false,
            FlightRoute = haNoiToHCMFlight.FlightRoute,
            DepartureTime = haNoiToHCMFlight.DepartureTime,
            ArrivalTime = haNoiToHCMFlight.ArrivalTime,
            Distance = haNoiToHCMFlightRoute.Distance
        };
        haNoiToHCMFSchedule.Flights.Add(haNoiToHCMFlight);

        var hCmToHanoiFSchedule = new Schedule()
        {
            HasTransit = true,
            FlightRoute = hCMToHaNoiFlightRoute,
            DepartureTime = hCMToDaNangFlight.DepartureTime,
            ArrivalTime = daNangToHaNoiFlight.ArrivalTime,
            Distance = hCMToDaNangFlightRoute.Distance + daNangToHaNoiFlightRoute.Distance
        };
        hCmToHanoiFSchedule.Flights.Add(hCMToDaNangFlight);
        hCmToHanoiFSchedule.Flights.Add(daNangToHaNoiFlight);

        foreach (Schedule schedule in new[] { haNoiToHCMFSchedule, hCmToHanoiFSchedule })
        {
            Price economyPrice = new()
            {
                SeatClass = economySeatClass,
                Schedule = schedule,
                Value = (_appDBContext.DefaultPrices.Where(p =>
                    p.SeatClass.Name == SeatEnums.Classes.Economy.ToString()
                    && p.FlightRoute == schedule.Flights[0].FlightRoute
                ).FirstOrDefault()?.Value ?? 0)
                + ((schedule.Flights.Count < 2 ? 0 : _appDBContext.DefaultPrices.Where(p =>
                    p.SeatClass.Name == SeatEnums.Classes.Economy.ToString()
                    && p.FlightRoute == schedule.Flights[1].FlightRoute
                ).FirstOrDefault()?.Value) ?? 0)
            };
            Price businessPrice = new()
            {
                SeatClass = businessSeatClass,
                Schedule = schedule,
                Value = (_appDBContext.DefaultPrices.Where(p =>
                    p.SeatClass.Name == SeatEnums.Classes.Business.ToString()
                    && p.FlightRoute == schedule.Flights[0].FlightRoute
                ).FirstOrDefault()?.Value ?? 0)
                + ((schedule.Flights.Count < 2 ? 0 : _appDBContext.DefaultPrices.Where(p =>
                    p.SeatClass.Name == SeatEnums.Classes.Business.ToString()
                    && p.FlightRoute == schedule.Flights[1].FlightRoute
                ).FirstOrDefault()?.Value) ?? 0)
            };
            _appDBContext.Add(economyPrice);
            _appDBContext.Add(businessPrice);
        }

        _appDBContext.Add(haNoiToHCMFSchedule);
        _appDBContext.Add(hCmToHanoiFSchedule);

        _appDBContext.SaveChanges();
    }
    static async Task RoutesSeed()
    {
        var airports = new Airport[]{
            new () {
            Name = "Hà Nội",
            NameEn = "Ha Noi"
            },
            new () {
            Name = "Đà Nẵng ",
            NameEn = "Da Nang"
            },
            new () {
            Name = "Hồ Chí Minh",
            NameEn = "Ho Chi Minh"
            },
            new () {
            Name = "Cần Thơ",
            NameEn = "Can Tho"
            },
        };

        _appDBContext.AddRange(airports);
        _appDBContext.SaveChanges();

        var flightRoutes = new[] {
            new FlightRoute() {
                DepartureAirport = airports[0],
                DestinationAirport = airports[1],
                Distance = 50
            },
            new FlightRoute() {
                DepartureAirport = airports[1],
                DestinationAirport = airports[2],
                Distance = 50
            },
            new FlightRoute() {
                DepartureAirport = airports[0],
                DestinationAirport = airports[2],
                Distance = 100
            },
            new FlightRoute() {
                DepartureAirport =  airports[1],
                DestinationAirport =airports[0],
                Distance = 50
            },
            new FlightRoute() {
                DepartureAirport =  airports[2],
                DestinationAirport =airports[1],
                Distance = 50
            },
            new FlightRoute() {
                DepartureAirport =  airports[2],
                DestinationAirport =airports[0],
                Distance = 100
            },
        };

        _appDBContext.AddRange(flightRoutes);

        _appDBContext.SaveChanges();
    }

    static async Task BookingsSeed()
    {

    }
}