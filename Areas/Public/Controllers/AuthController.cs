using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Public.Controllers;

[Route("/auth")]
public class AuthController : AppBaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDBContext _appDBContext;
    private readonly HttpContext _httpContext;

    public AuthController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        AppDBContext appDBContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _appDBContext = appDBContext;
        _httpContext = httpContextAccessor.HttpContext;
    }


    [HttpGet("/login")]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    // [HttpGet("login")]
    // public async Task<IActionResult> Login([FromForm] LoginModel model)
    // {
    //     User? user = await _appDBContext.Users
    //                 .Where(user =>
    //                     user.NormalizedEmail == model.Email
    //                     || user.PhoneNumber == model.Email)
    //                 .FirstOrDefaultAsync();

    //     if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
    //     {
    //         return Unauthorized(new ResponseContent
    //         {
    //             Title = "Not found any user with this email or phone number",
    //         });
    //     }

    //     var userRoles = await _userManager.GetRolesAsync(user);

    //     var authClaims = new List<Claim>
    //             {
    //                 new Claim(ClaimTypes.Name, user.UserName),
    //                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //             };

    //     foreach (var userRole in userRoles)
    //     {
    //         authClaims.Add(new Claim(ClaimTypes.Role, userRole));
    //     }

    //     var token = JwtTokenHelper.GenerateToken(authClaims, _configuration);

    //     return Ok(new ResponseContent
    //     {
    //         Title = "Login successfully",
    //         Data = new
    //         {
    //             token = new JwtSecurityTokenHandler().WriteToken(token),
    //             expiration = token.ValidTo
    //         },
    //     });
    // }

    // [HttpPost("register")]
    // public async Task<IActionResult> Register([FromForm] RegisterModel model)
    // {
    //     var userExists = await _userManager.FindByEmailAsync(model.Email);
    //     if (userExists != null)
    //         return Conflict(new ResponseContent
    //         {
    //             Title = "User with the same email already exists.",
    //         });

    //     User user = new User()
    //     {
    //         Email = model.Email,
    //         PhoneNumber = model.PhoneNumber,
    //         FirstName = model.FirstName,
    //         LastName = model.LastName,
    //         DateOfBirth = model.DateOfBirth,
    //         Gender = model.Gender,
    //         IdNumber = model.IdNumber,
    //         CityId = model.CityId,
    //         DistrictId = model.DistrictId,
    //         WardId = model.WardId,
    //         Address = model.Address,
    //         Address2 = model.Address2,
    //         SecurityStamp = Guid.NewGuid().ToString(),
    //     };
    //     user.UserName = user.Id;

    //     var result = await _userManager.CreateAsync(user, model.Password);
    //     if (!result.Succeeded)
    //         return StatusCode(StatusCodes.Status500InternalServerError,
    //          new ResponseContent
    //          {
    //              Status = "Error",
    //              Title = "User creation failed! Please check user details and try again."
    //          });

    //     return Ok(new ResponseContent
    //     {
    //         Title = "User created successfully!",
    //     });
    // }

    // [HttpGet("get-me")]
    // [Authorize]
    // public async Task<IActionResult> GetMe()
    // {
    //     var currentUser = await _userManager.FindByNameAsync(_httpContext.User.Identity.Name);
    //     return Ok(currentUser);
    // }

    // [HttpGet("test")]
    // // [Authorize]
    // public async Task<IActionResult> Test()
    // {
    //     // var currentUser = await _userManager.FindByNameAsync(_httpContext.User.Identity.Name);
    //     // var userRoles = await _userManager.GetRolesAsync(currentUser);

    //     await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
    //     await _roleManager.CreateAsync(new IdentityRole(UserRoles.Staff));
    //     await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

    //     return Ok(User.Identity);
    // }


}