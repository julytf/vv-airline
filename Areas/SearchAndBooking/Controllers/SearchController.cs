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
        return RedirectToAction("FlightsSelection");
    }

    [HttpGet("flights-selection")]
    public async Task<IActionResult> FlightsSelection()
    {
        throw new NotImplementedException();
    }

    [HttpGet("seats-selection")]
    public async Task<IActionResult> SeatsSelection()
    {
        throw new NotImplementedException();
    }

    [HttpGet("addition-services-selection")]
    public async Task<IActionResult> AdditionServicesSelection()
    {
        throw new NotImplementedException();
    }

    [HttpGet("passengers-information")]
    public async Task<IActionResult> PassengersInformation()
    {
        throw new NotImplementedException();
    }

    [HttpGet("checkout")]
    public async Task<IActionResult> Checkout()
    {
        throw new NotImplementedException();
    }
}

