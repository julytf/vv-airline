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
[Route("/admin/aircrafts")]
[Authorize]
public class AircraftsController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AircraftsController(
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
        List<Aircraft> aircrafts = _appDBContext
            .Aircrafts
            .Include(ac => ac.Model)
            .ToList();
        // Console.WriteLine(JsonConvert.SerializeObject(aircrafts));
        ViewBag.aircrafts = aircrafts;
        return View();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        List<Model> models = _appDBContext
            .Models
            .ToList();

        ViewBag.models = models;
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(AircraftModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Aircraft newAircraft = new()
        {
            RegistrationNumber = model.RegistrationNumber,
            Name = model.Name,
            Model = _appDBContext.Models.Single(m => m.Id == model.ModelId),
            Status = model.Status,
        };

        _appDBContext.Add(newAircraft);
        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> Show(int id)
    // {
    //     throw new NotImplementedException();
    // }

    [HttpGet("{id}")]
    public async Task<IActionResult> Update(string id)
    {
        Aircraft aircraft = _appDBContext
            .Aircrafts
            .Include(ac => ac.Model)
            .First(ac => ac.RegistrationNumber == id);

        List<Model> models = _appDBContext
            .Models
            .ToList();

        AircraftModel model = new()
        {
            RegistrationNumber = aircraft.RegistrationNumber,
            Name = aircraft.Name,
            ModelId = aircraft.Model.Id,
            Status = aircraft.Status,
        };

        ViewBag.models = models;
        return View(model);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(string id, AircraftModel model)
    {
        List<Model> models = _appDBContext
            .Models
            .ToList();

        ViewBag.models = models;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Aircraft aircraft = _appDBContext
            .Aircrafts
            .First(ac => ac.RegistrationNumber == id);

        aircraft.Name = model.Name;
        aircraft.Model = _appDBContext.Models.Single(m => m.Id == model.ModelId);
        aircraft.Status = model.Status;


        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}