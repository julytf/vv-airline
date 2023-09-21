using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin")]
public class AuthController : AppBaseController
{
    public AuthController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}