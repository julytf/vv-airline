using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vv_airline.Areas.Admin.Models;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin/flights")]
[Authorize]
public class FlightsController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public FlightsController(
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
        List<Flight> flights = _appDBContext
            .Flights
            .Include(f => f.Aircraft)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .ToList();

        ViewBag.flights = flights;
        return View();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        List<Aircraft> aircrafts = _appDBContext
            .Aircrafts
            .ToList();
        List<FlightRoute> flightRoutes = _appDBContext
            .FlightRoutes
            .Include(fr => fr.DepartureAirport)
            .Include(fr => fr.DestinationAirport)
            .ToList();

        ViewBag.aircrafts = aircrafts;
        ViewBag.flightRoutes = flightRoutes;
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(FlightModel model)
    {
        if (!ModelState.IsValid)
        {
            List<Aircraft> aircrafts = _appDBContext
                .Aircrafts
                .ToList();
            List<FlightRoute> flightRoutes = _appDBContext
                .FlightRoutes
                .Include(fr => fr.DepartureAirport)
                .Include(fr => fr.DestinationAirport)
                .ToList();

            ViewBag.aircrafts = aircrafts;
            ViewBag.flightRoutes = flightRoutes;
            return View(model);
        }

        Flight newFlight = new()
        {
            Aircraft = _appDBContext.Aircrafts.First(a => a.RegistrationNumber == model.AircraftRegistrationNumber),
            DepartureTime = model.DepartureTime,
            ArrivalTime = model.ArrivalTime,
            RemainingSeats = model.RemainingSeats,
            FlightRoute = _appDBContext.FlightRoutes.First(a => a.Id == model.FlightRouteId),
            Status = model.Status,
        };

        _appDBContext.Add(newFlight);
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
        Flight flight = _appDBContext
            .Flights
            .Include(f => f.Aircraft)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .First(f => f.Id == id);

        List<Aircraft> aircrafts = _appDBContext
            .Aircrafts
            .ToList();
        List<FlightRoute> flightRoutes = _appDBContext
            .FlightRoutes
            .Include(fr => fr.DepartureAirport)
            .Include(fr => fr.DestinationAirport)
            .ToList();


        FlightModel model = new()
        {
            AircraftRegistrationNumber = flight.Aircraft.RegistrationNumber,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            RemainingSeats = flight.RemainingSeats,
            FlightRouteId = flight.FlightRoute.Id,
            Status = flight.Status,
        };

        ViewBag.aircrafts = aircrafts;
        ViewBag.flightRoutes = flightRoutes;
        return View(model);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(long id, FlightModel model)
    {

        if (!ModelState.IsValid)
        {

            List<Aircraft> aircrafts = _appDBContext
                .Aircrafts
                .ToList();
            List<FlightRoute> flightRoutes = _appDBContext
                .FlightRoutes
                .Include(fr => fr.DepartureAirport)
                .Include(fr => fr.DestinationAirport)
                .ToList();

            ViewBag.aircrafts = aircrafts;
            ViewBag.flightRoutes = flightRoutes;
            return View(model);
        }

        Flight flight = _appDBContext
            .Flights
            .First(f => f.Id == id);


        flight.Aircraft = _appDBContext.Aircrafts.First(a => a.RegistrationNumber == model.AircraftRegistrationNumber);
        flight.DepartureTime = model.DepartureTime;
        flight.ArrivalTime = model.ArrivalTime;
        flight.RemainingSeats = model.RemainingSeats;
        flight.FlightRoute = _appDBContext.FlightRoutes.First(a => a.Id == model.FlightRouteId);
        flight.Status = model.Status;

        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}