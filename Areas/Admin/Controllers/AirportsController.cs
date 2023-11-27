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
[Route("/admin/airports")]
[Authorize]
public class AirportsController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AirportsController(
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
        List<Airport> airports = _appDBContext
            .Airports
            .Include(ap => ap.Province)
            .Include(ap => ap.District)
            .Include(ap => ap.Ward)
            .ToList();

        ViewBag.airports = airports;
        return View();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(AirportModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Airport newAirport = new()
        {
            Name = model.Name,
            Province = _appDBContext.Provinces.First(p => p.Code == model.ProvinceCode),
            District = _appDBContext.Districts.First(d => d.Code == model.DistrictCode),
            Ward = _appDBContext.Wards.First(w => w.Code == model.WardCode),
        };

        _appDBContext.Add(newAirport);
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
        Airport airport = _appDBContext
            .Airports
            .Include(ap => ap.Province)
            .Include(ap => ap.District)
            .Include(ap => ap.Ward)
            .First(ap => ap.Id == id);


        AirportModel model = new()
        {
            Name = airport.Name,
            ProvinceCode = airport?.Province?.Code,
            DistrictCode = airport?.District?.Code,
            WardCode = airport?.Ward?.Code,
        };

        return View(model);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(long id, AirportModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Airport airport = _appDBContext
            .Airports
            .First(ap => ap.Id == id);

        airport.Name = model.Name;
        airport.Province = _appDBContext.Provinces.FirstOrDefault(p => p.Code == model.ProvinceCode);
        airport.District = _appDBContext.Districts.FirstOrDefault(d => d.Code == model.DistrictCode);
        airport.Ward = _appDBContext.Wards.FirstOrDefault(w => w.Code == model.WardCode);

        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}