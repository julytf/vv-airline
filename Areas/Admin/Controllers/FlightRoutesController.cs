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
[Route("/admin/flightRoutes")]
[Authorize]
public class FlightRoutesController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public FlightRoutesController(
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
        List<FlightRoute> flightRoutes = _appDBContext
            .FlightRoutes
            .Include(fr => fr.DepartureAirport)
            .Include(fr => fr.DestinationAirport)
            .ToList();

        ViewBag.flightRoutes = flightRoutes;
        return View();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        List<Airport> airports = _appDBContext
            .Airports
            .ToList();

        ViewBag.airports = airports;
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(FlightRouteModel model)
    {
        List<Airport> airports = _appDBContext
            .Airports
            .ToList();

        ViewBag.airports = airports;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        FlightRoute newFlightRoute = new()
        {
            DepartureAirport = _appDBContext.Airports.First(ap => ap.Id == model.DepartureAirportId),
            DestinationAirport = _appDBContext.Airports.First(ap => ap.Id == model.DestinationAirportId),
            Distance = model.Distance,
        };

        _appDBContext.Add(newFlightRoute);
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
        List<Airport> airports = _appDBContext
            .Airports
            .ToList();

        ViewBag.airports = airports;

        FlightRoute flightRoute = _appDBContext
            .FlightRoutes
            .Include(fr => fr.DepartureAirport)
            .Include(fr => fr.DestinationAirport)
            .First(fr => fr.Id == id);

        FlightRouteModel model = new()
        {
            DepartureAirportId = flightRoute.DepartureAirport.Id,
            DestinationAirportId = flightRoute.DestinationAirport.Id,
            Distance = flightRoute.Distance,
        };

        return View(model);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(long id, FlightRouteModel model)
    {
        List<Airport> airports = _appDBContext
            .Airports
            .ToList();

        ViewBag.airports = airports;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        FlightRoute flightRoute = _appDBContext
            .FlightRoutes
            .First(fr => fr.Id == id);


        flightRoute.DepartureAirport = _appDBContext.Airports.First(ap => ap.Id == model.DepartureAirportId);
        flightRoute.DestinationAirport = _appDBContext.Airports.First(ap => ap.Id == model.DestinationAirportId);
        flightRoute.Distance = model.Distance;

        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}