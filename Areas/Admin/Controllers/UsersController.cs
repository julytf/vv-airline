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
[Route("/admin/users")]
[Authorize]
public class UsersController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public UsersController(
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
        List<User> users = _appDBContext
            .Users
            .Include(u => u.Bookings)
            .ToList();
        // Console.WriteLine(JsonConvert.SerializeObject(users));
        ViewBag.users = users;
        return View();
    }

    [HttpGet("flights")]
    public async Task<IActionResult> BookingsHistory()
    {
        List<Booking> bookings = _appDBContext
            .Bookings
            .Include(b => b.User)
            .Include(b => b.Passengers)
                .ThenInclude(p => p.Tickets)
                    .ThenInclude(t => t.Seat)
                        .ThenInclude(s => s.SeatClass)
            .Include(b => b.Schedules)
                .ThenInclude(b => b.Flights)
                    .ThenInclude(f => f.Aircraft)
            .Include(b => b.Schedules)
                .ThenInclude(b => b.Flights)
                    .ThenInclude(b => b.FlightRoute)
                        .ThenInclude(fr => fr.DepartureAirport)
            .Include(b => b.Schedules)
                .ThenInclude(b => b.Flights)
                    .ThenInclude(b => b.FlightRoute)
                        .ThenInclude(fr => fr.DestinationAirport)
            .Include(b => b.Schedules)
                .ThenInclude(b => b.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
            .Include(b => b.Schedules)
                .ThenInclude(b => b.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
            .ToList();
        // Console.WriteLine(JsonConvert.SerializeObject(users));
        ViewBag.bookings = bookings;
        return View();
    }

    // [HttpGet("create")]
    // public async Task<IActionResult> Create()
    // {
    //     List<Model> models = _appDBContext
    //         .Models
    //         .ToList();

    //     ViewBag.models = models;
    //     return View();
    // }

    // [HttpPost("create")]
    // public async Task<IActionResult> Create(UserModel model)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(model);
    //     }

    //     User newUser = new()
    //     {
    //     };

    //     _appDBContext.Add(newUser);
    //     _appDBContext.SaveChanges();

    //     return RedirectToAction("Index");
    // }

    // // [HttpGet("{id}")]
    // // public async Task<IActionResult> Show(int id)
    // // {
    // //     throw new NotImplementedException();
    // // }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> Update(string id)
    // {
    //     User user = _appDBContext
    //         .Users
    //         .First(ac => ac.RegistrationNumber == id);

    //     List<Model> models = _appDBContext
    //         .Models
    //         .ToList();

    //     UserModel model = new()
    //     {
    //     };

    //     ViewBag.models = models;
    //     return View(model);
    // }

    // [HttpPost("{id}")]
    // public async Task<IActionResult> Update(string id, UserModel model)
    // {
    //     List<Model> models = _appDBContext
    //         .Models
    //         .ToList();

    //     ViewBag.models = models;

    //     if (!ModelState.IsValid)
    //     {
    //         return View(model);
    //     }

    //     User user = _appDBContext
    //         .Users
    //         .First(ac => ac.RegistrationNumber == id);



    //     _appDBContext.SaveChanges();

    //     return RedirectToAction("Index");
    // }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     throw new NotImplementedException();
    // }
}