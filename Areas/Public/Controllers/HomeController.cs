using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vv_airline.Areas.Public.Models;
using vv_airline.Controllers;
using vv_airline.Models;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Public.Controllers;

[Area("Public")]
[Route("/")]
public class HomeController : AppBaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager
     )
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // throw new NotImplementedException();
        return View();
    }

    [HttpGet("/test")]
    public async Task<IActionResult> Test()
    {
        // _userManager.FindByNameAsync
        // var session = context.Session;

        // TestModel testModel = new TestModel()
        // {
        //     title = "test title",
        //     value = "1111"
        // };

        // Session["testModel"] = testModel;

        return Json(new
        {
            value = _signInManager.IsSignedIn(User)
        });
    }

}
