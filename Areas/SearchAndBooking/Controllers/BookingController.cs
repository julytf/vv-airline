using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.SearchAndBooking.Controllers;

[Area("SearchAndBooking")]
[Route("/bookings")]
public class BookingController : AppBaseController
{
    public BookingController()
    {
    }

// show user flights
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ShowView(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("book-flights")]
    public async Task<IActionResult> BookFlights()
    {
        throw new NotImplementedException();
    }
}