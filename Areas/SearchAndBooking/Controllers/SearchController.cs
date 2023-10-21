using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.SearchAndBooking.Controllers;

[Area("SearchAndBooking")]
[Route("/search")]
public class SearchController : AppBaseController
{
    public SearchController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("schedules-selection")]
    public async Task<IActionResult> SchedulesSelection()
    {
        return View();
    }

    [HttpGet("seats-selection")]
    public async Task<IActionResult> SeatsSelection()
    {
        return View();
    }

    [HttpGet("addition-services-selection")]
    public async Task<IActionResult> AdditionServicesSelection()
    {
        throw new NotImplementedException();
    }

    [HttpGet("passengers-information")]
    public async Task<IActionResult> PassengersInformation()
    {
        return View();
    }

    [HttpGet("checkout")]
    public async Task<IActionResult> Checkout()
    {
        throw new NotImplementedException();
    }
}

