using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vv_airline.Areas.Public.Models;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Public.Controllers;

[Area("Public")]
[Route("/account")]
public class AccountController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    // private readonly RoleManager<IdentityRole> _roleManager;
    // private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    // private readonly HttpContext _httpContext;
    // private readonly SignInManager<User> _signInManager;
    // private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<User> userManager,
        // RoleManager<IdentityRole> roleManager,
        // SignInManager<User> signInManager,
        // IConfiguration configuration,
        AppDBContext appDBContext
        // IHttpContextAccessor httpContextAccessor,
        // ILogger<AccountController> logger
        )
    {
        _userManager = userManager;
        // _roleManager = roleManager;
        // _signInManager = signInManager;
        // _configuration = configuration;
        _appDBContext = appDBContext;
        // _httpContext = httpContextAccessor.HttpContext;
        // _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        User currentUser = _appDBContext
            .Users
            .Include(u => u.Province)
            .Include(u => u.District)
            .Include(u => u.Ward)
            .Single(u => u.UserName == User.Identity.Name);

        UserModel model = new()
        {
            Email = currentUser.Email,
            PhoneNumber = currentUser.PhoneNumber,
            // Password = "",
            FirstName = currentUser.FirstName,
            LastName = currentUser.LastName,
            DateOfBirth = currentUser.DateOfBirth,
            Gender = currentUser.Gender,
            IdNumber = currentUser.IdNumber,
            ProvinceCode = currentUser.Province?.Code,
            DistrictCode = currentUser.District?.Code,
            WardCode = currentUser.Ward?.Code,
            Address = currentUser.Address,
            Address2 = currentUser.Address2,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UserModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Dữ liệu không hợp lệ.");
            // foreach (var modelState in ModelState.Values)
            // {
            //     foreach (ModelError error in modelState.Errors)
            //     {
            //         Console.WriteLine(error.ErrorMessage);
            //     }
            // }
            return View(model);
        }

        User currentUser = _appDBContext
            .Users
            .Include(u => u.Province)
            .Include(u => u.District)
            .Include(u => u.Ward)
            .Single(u => u.UserName == User.Identity.Name);

        currentUser.Email = model.Email;
        currentUser.PhoneNumber = model.PhoneNumber;
        currentUser.FirstName = model.FirstName;
        currentUser.LastName = model.LastName;
        currentUser.DateOfBirth = model.DateOfBirth;
        currentUser.Gender = model.Gender;
        currentUser.IdNumber = model.IdNumber;
        currentUser.Province = _appDBContext.Provinces.Where(p => p.Code == model.ProvinceCode).FirstOrDefault();
        currentUser.District = _appDBContext.Districts.Where(d => d.Code == model.DistrictCode).FirstOrDefault();
        currentUser.Ward = _appDBContext.Wards.Where(w => w.Code == model.WardCode).FirstOrDefault();
        currentUser.Address = model.Address;
        currentUser.Address2 = model.Address2;

        var result = await _userManager.UpdateAsync(currentUser);

        if (!result.Succeeded)
        {

            ModelState.AddModelError(string.Empty, "Tạo không thành công");
            return View(model);
        }

        // _logger.LogInformation("Đã tạo user mới.");

        return View();
    }

    // [HttpPost("/update")]
    // public async Task<IActionResult> Update()
    // {
    //     throw new NotImplementedException();
    // }

    [HttpGet("/change-password")]
    public async Task<IActionResult> ChangePassword(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("/change-password")]
    public async Task<IActionResult> ChangePassword()
    {
        throw new NotImplementedException();
    }
}