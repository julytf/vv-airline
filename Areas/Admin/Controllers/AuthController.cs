using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vv_airline.Areas.Admin.Models;
using vv_airline.Controllers;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Areas.Admin.Controllers;


// [Authorize(AuthenticationSchemes = "AdminCookie")]
[Area("Admin")]
[Route("/admin")]
public class AuthController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AuthController(
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

    // [HttpGet]
    // public async Task<IActionResult> Index()
    // {
    //     throw new NotImplementedException();
    // }
    [HttpGet("login")]
    // [AllowAnonymous]
    // [Authorize]
    public async Task<IActionResult> Login()
    {
        return View();
    }



    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl = "/admin")
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Không tìm thấy tài khoản với email này.");
            return View(model);
        }
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(UserEnums.Roles.Admin.ToString()))
        {
            ModelState.AddModelError(string.Empty, "Tài khoản này không có quyền đăng nhập trang quản lý    .");
            return View(model);
        }


        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
            return View(model);
        }

        if (result.RequiresTwoFactor)
        {
            throw new NotImplementedException();
            // return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning(2, "Tài khoản bị khóa");
            return View("Lockout");
        }


        _logger.LogInformation(1, "Đăng nhập thành công");
        // return Json(new
        // {
        //     result.Succeeded,
        //     User.Identity.IsAuthenticated
        // });
        return LocalRedirect(returnUrl);

    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        // await HttpContext.SignOutAsync("UserCookie");
        _logger.LogInformation("User đăng xuất");
        return RedirectToAction("Login", "Auth");
    }

    // [HttpGet("test")]
    // [AllowAnonymous]
    // [Authorize(AuthenticationSchemes = "AdminCookie")]
    // public async Task<IActionResult> test()
    // {
    //     // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);

    //     User admin = _appDBContext.Users.First(u => u.UserName == "admin");

    //     var claims = new List<Claim>
    //         {
    //             new Claim(ClaimTypes.Name, admin.UserName),
    //         };

    //     var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    //     var principal = new ClaimsPrincipal(identity);

    //     await HttpContext.SignInAsync("AdminCookie", principal);
    //     return Json(new
    //     {
    //         name = User.Identity.Name,
    //         id = User.Identities,
    //     });
    // }
}