using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Public.Controllers;

[Area("Public")]
[Route("/flights")]
public class BookingController : AppBaseController
{
    private readonly AppDBContext _appDBContext;
    public BookingController(
        AppDBContext appDBContext
        )
    {
        _appDBContext = appDBContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // User user = _appDBContext
        //     .Users
        //     .Include(u => u.Bookings)
        //     .Where(u => u.UserName == User.Identity.Name).Single();
        User user = _appDBContext
            .Users
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Passengers)
                    .ThenInclude(p => p.Tickets)
                        .ThenInclude(t => t.Seat)
                            .ThenInclude(s => s.SeatClass)
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Schedules)
                    .ThenInclude(b => b.Flights)
                        .ThenInclude(f => f.Aircraft)
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Schedules)
                    .ThenInclude(b => b.Flights)
                        .ThenInclude(b => b.FlightRoute)
                            .ThenInclude(fr => fr.DepartureAirport)
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Schedules)
                    .ThenInclude(b => b.Flights)
                        .ThenInclude(b => b.FlightRoute)
                            .ThenInclude(fr => fr.DestinationAirport)
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Schedules)
                    .ThenInclude(b => b.FlightRoute)
                        .ThenInclude(fr => fr.DepartureAirport)
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Schedules)
                    .ThenInclude(b => b.FlightRoute)
                        .ThenInclude(fr => fr.DestinationAirport)
            .Single(u => u.UserName == User.Identity.Name);


        // Console.WriteLine(user.UserName);
        // Console.WriteLine(user.Bookings.Count);

        ViewBag.bookings = user.Bookings;
        return View();
    }

    // [HttpGet("/update")]
    // public async Task<IActionResult> Update(int id)
    // {
    //     throw new NotImplementedException();
    // }

    // [HttpPost("/update")]
    // public async Task<IActionResult> Update()
    // {
    //     throw new NotImplementedException();
    // }

    // [HttpGet("/change-password")]
    // public async Task<IActionResult> ChangePassword(int id)
    // {
    //     throw new NotImplementedException();
    // }

    // [HttpPost("/change-password")]
    // public async Task<IActionResult> ChangePassword()
    // {
    //     throw new NotImplementedException();
    // }
}