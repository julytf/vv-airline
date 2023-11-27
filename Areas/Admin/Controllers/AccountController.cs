using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin/account")]
[Authorize]
public class AccountController : AppBaseController
{
    public AccountController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        throw new NotImplementedException();
    }

    [HttpGet("/update")]
    public async Task<IActionResult> Update(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("/update")]
    public async Task<IActionResult> Update()
    {
        throw new NotImplementedException();
    }

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