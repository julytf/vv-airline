using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vv_airline.Areas.Admin.Models;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin/models")]
public class ModelsController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public ModelsController(
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
        List<Model> models = _appDBContext
            .Models
            .ToList();
        ViewBag.models = models;
        return View();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ModelModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Model newModel = new()
        {
            Name = model.Name
        };

        _appDBContext.Add(newModel);
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
        Model existedModel = _appDBContext
            .Models
            .First(m => m.Id == id);

        ModelModel model = new()
        {
            Name = existedModel.Name
        };

        return View(model);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(long id, ModelModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Model existedModel = _appDBContext
            .Models
            .First(m => m.Id == id);

        existedModel.Name = model.Name;

        _appDBContext.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}