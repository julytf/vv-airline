using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using vv_airline.Areas.Public.Models;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Public.Controllers;

[Area("Public")]
[Route("/auth")]
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


    [HttpGet("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl = "/")
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
        Console.WriteLine(await _signInManager.CheckPasswordSignInAsync(user, model.Password, false));
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
            return View(model);
        }

        // if (result.RequiresTwoFactor)
        // {
        //     return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        // }

        if (result.IsLockedOut)
        {
            _logger.LogWarning(2, "Tài khoản bị khóa");
            return View("Lockout");
        }

        _logger.LogInformation(1, "User logged in.");
        return LocalRedirect(returnUrl);

    }

    [HttpPost("/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User đăng xuất");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/register")]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    //
    // POST: /Account/Register
    [HttpPost("/register")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model, string returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User()
        {
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            IdNumber = model.IdNumber,
            CityId = model.CityId,
            DistrictId = model.DistrictId,
            WardId = model.WardId,
            Address = model.Address,
            Address2 = model.Address2,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        user.UserName = user.Id;

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {

            ModelState.AddModelError(string.Empty, "Tạo không thành công");
            return View(model);
        }

        _logger.LogInformation("Đã tạo user mới.");

        // Phát sinh token để xác nhận email
        // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // https://localhost:5001/confirm-email?userId=fdsfds&code=xyz&returnUrl=
        // var callbackUrl = Url.ActionLink(
        //     action: nameof(ConfirmEmail),
        //     values:
        //         new
        //         {
        //             area = "Identity",
        //             userId = user.Id,
        //             code = code
        //         },
        //     protocol: Request.Scheme);

        // await _emailSender.SendEmailAsync(model.Email,
        //             "Xác nhận địa chỉ email",
        //             @$"Bạn đã đăng ký tài khoản trên RazorWeb, 
        //                    hãy <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bấm vào đây</a> 
        //                    để kích hoạt tài khoản.");

        // if (_userManager.Options.SignIn.RequireConfirmedAccount)
        // {
        //     return LocalRedirect(Url.Action(nameof(RegisterConfirmation)));
        // }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(returnUrl);
    }
}