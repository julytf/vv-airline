using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using vv_airline.Models;

namespace vv_airline.Controllers;

[Route("/")]
public class HomeController : AppBaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

}
