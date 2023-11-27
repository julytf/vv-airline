using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vv_airline.Areas.Admin.Models;
using vv_airline.Controllers;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin/schedules")]
[Authorize]
public class SchedulesController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public SchedulesController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        AppDBContext appDBContext,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AccountController> logger
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _appDBContext = appDBContext;
        _httpContext = httpContextAccessor.HttpContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<Schedule> schedules = _appDBContext
            .Schedules
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
            .ToList();
        // Console.WriteLine(JsonConvert.SerializeObject(schedules));
        ViewBag.schedules = schedules;
        return View();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        List<Flight> flights = _appDBContext
            .Flights
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .ToList();

        ViewBag.flights = flights;
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ScheduleModel model)
    {
        if (model.FlightIds[0] == null)
        {
            ModelState.AddModelError("FlightIds[0]", "Không thể trống");
        }
        if (!ModelState.IsValid)
        {
            List<Flight> flights = _appDBContext
                .Flights
                .Include(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
                .Include(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
                .ToList();

            ViewBag.flights = flights;
            model = new();
            return View(model);
        }

        Flight flightLeg1 = _appDBContext
            .Flights
            .Include(f => f.FlightRoute)
            .First(f => f.Id == model.FlightIds[0]);
        Flight? flightLeg2 = null!;
        if (model?.FlightIds[1] != null)
        {
            flightLeg2 = _appDBContext
                .Flights
                .Include(f => f.FlightRoute)
                .First(f => f.Id == model.FlightIds[1]);
        }
        // Console.WriteLine();
        Schedule newSchedule = new()
        {
            HasTransit = model?.FlightIds?[1] != null,
            DepartureTime = flightLeg1.DepartureTime,
            ArrivalTime = flightLeg2?.ArrivalTime ?? flightLeg1.ArrivalTime,
            Distance = flightLeg1.FlightRoute.Distance + (flightLeg2?.FlightRoute?.Distance ?? 0),
        };

        newSchedule.Flights.Add(flightLeg1);
        if (model?.FlightIds?[1] != null)
        {
            newSchedule.Flights.Add(flightLeg2);
        }

        if (model.FlightIds?[1] == null) {
            newSchedule.FlightRoute = flightLeg1.FlightRoute;
        } else {
            FlightRoute flightRoute = _appDBContext.FlightRoutes.First(fr => 
            fr.DepartureAirport == flightLeg1.FlightRoute.DepartureAirport
            && fr.DestinationAirport == flightLeg2.FlightRoute.DestinationAirport
            );
            newSchedule.FlightRoute = flightLeg1.FlightRoute;
        }

        var economySeatClass = _appDBContext.
            SeatClasses.
            Where(sl => sl.Name == SeatEnums.Classes.Economy.ToString()).
            Single();
        var businessSeatClass = _appDBContext.
            SeatClasses.
            Where(sl => sl.Name == SeatEnums.Classes.Business.ToString()).
            Single();

        Price[] prices = new[] {
            new Price() {
                Schedule = newSchedule,
                SeatClass = economySeatClass,
                Value = model.EconomyPrice
            },
            new Price() {
                Schedule = newSchedule,
                SeatClass = businessSeatClass,
                Value = model.BusinessPrice
            }
        };

        _appDBContext.AddRange(prices);
        _appDBContext.Add(newSchedule);
        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> Show(int id)
    // {
    //     throw new NotImplementedException();
    // }

    [HttpGet("{id}")]
    public async Task<IActionResult> Update(long id)
    {
        Schedule schedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
            .First(ac => ac.Id == id);

        List<Flight> flights = _appDBContext
            .Flights
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .ToList();

        ScheduleModel model = new()
        {
            FlightIds = new long?[2],
        };
        model.FlightIds[0] = schedule.Flights[0].Id;
        if (schedule.Flights.Count == 2)
            model.FlightIds[1] = schedule.Flights[1].Id;

        ViewBag.flights = flights;
        return View(model);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(long id, ScheduleModel model)
    {

        if (!ModelState.IsValid)
        {
            List<Flight> flights = _appDBContext
                .Flights
                .Include(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
                .Include(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
                .ToList();

            ViewBag.flights = flights;
            return View(model);
        }

        Schedule schedule = _appDBContext
            .Schedules
            .First(ac => ac.Id == id);

        Flight flightLeg1 = _appDBContext
            .Flights
            .Include(f => f.FlightRoute)
            .First(f => f.Id == model.FlightIds[0]);
        Flight? flightLeg2 = null!;
        if (model?.FlightIds[1] != null)
        {
            flightLeg2 = _appDBContext
                .Flights
                .Include(f => f.FlightRoute)
                .First(f => f.Id == model.FlightIds[1]);
        }

        schedule.HasTransit = model.FlightIds[1] != null;
        schedule.DepartureTime = flightLeg1.DepartureTime;
        schedule.ArrivalTime = flightLeg2?.ArrivalTime ?? flightLeg1.ArrivalTime;
        schedule.Distance = flightLeg1.FlightRoute.Distance + (flightLeg2?.FlightRoute?.Distance ?? 0);

        schedule.Flights = new List<Flight>();
        schedule.Flights.Add(flightLeg1);
        if (model?.FlightIds[1] != null)
        {
            schedule.Flights.Add(flightLeg2);
        }

        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}