using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
using vv_airline.Areas.SearchAndBooking.Models.Services;
using vv_airline.Controllers;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Areas.SearchAndBooking.Controllers;

[Area("SearchAndBooking")]
[Route("/bookings")]
public class BookingController : AppBaseController
{
    private readonly AppDBContext _appDBContext;
    private readonly SearchAndBookingService _searchAndBookingService;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public BookingController(
        AppDBContext appDBContext,
        SearchAndBookingService searchAndBookingService,
        UserManager<User> userManager,
        IConfiguration configuration
        )
    {
        _appDBContext = appDBContext;
        _searchAndBookingService = searchAndBookingService;
        _userManager = userManager;
        _configuration = configuration;
    }


    [HttpGet("redirect-to-stripe")]
    public IActionResult RedirectToStripe()
    {
        StripeConfiguration.ApiKey = _configuration["Stripe:Secret"];


        Schedule goSchedule;
        Schedule? returnSchedule = null!;
        goSchedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
            .Single(s => s.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId);
        if (_searchAndBookingService.Data.Search.IsRoundtrip)
        {
            returnSchedule = _appDBContext
                .Schedules
                .Include(s => s.Flights)
                .SingleOrDefault(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.ScheduleId);
        }

        vv_airline.Models.Data.Price goPrice = _appDBContext
            .Prices
            .Single(p =>
                p.Schedule == goSchedule
                && p.SeatClass.Name == _searchAndBookingService.Data.GoSchedule.ClassName
            );

        vv_airline.Models.Data.Price? returnPrice = null!;
        if (_searchAndBookingService.Data.ReturnSchedule != null)
            returnPrice = _appDBContext
                .Prices
                .Single(p =>
                    p.Schedule == returnSchedule
                    && p.SeatClass.Name == _searchAndBookingService.Data.ReturnSchedule.ClassName
                );

        long totalPrice = (goPrice.Value + (returnPrice?.Value ?? 0))
        * (_searchAndBookingService.Data.Search.Adults + _searchAndBookingService.Data.Search.Children);


        string SearchAndBookingServiceDataJson = JsonConvert.SerializeObject(_searchAndBookingService.Data);
        // Console.WriteLine(SearchAndBookingServiceDataJson);
        _searchAndBookingService.Data = new();
        _searchAndBookingService.SaveToSession();

        BookingSession bookingSession = new()
        {
            Data = SearchAndBookingServiceDataJson
        };


        _appDBContext.Add(bookingSession);
        _appDBContext.SaveChanges();


        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = totalPrice,
                        Currency = "vnd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "VV Airline airline tickets",
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "http://localhost:8080/bookings/success?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = "http://localhost:8080/bookings/cancel",
            Metadata = new Dictionary<string, string>
            {
                { "Id", bookingSession.Id.ToString() },
            },
        };

        var service = new SessionService();
        var session = service.Create(options);

        // return Json(new { session = session });
        return Redirect(session.Url);
    }

    [HttpGet("stripe-webhook")]
    [HttpGet("checkout-success")]
    [HttpGet("success")]
    public IActionResult CheckoutSuccess(string session_id)
    {
        StripeConfiguration.ApiKey = _configuration["Stripe:Secret"];

        var service = new SessionService();
        var session = service.Get(session_id);

        if (session.PaymentStatus != "paid")
        {
            return RedirectToAction("Index", "Search");
        }

        BookingSession bookingSession = _appDBContext.BookingSessions.First(bs => bs.Id == long.Parse(session.Metadata["Id"]));


        string SearchAndBookingServiceDataJson = bookingSession.Data;
        // TODO:

        _appDBContext.Remove(bookingSession);
        _appDBContext.SaveChanges();

        SearchAndBookingModel Data = JsonConvert.DeserializeObject<SearchAndBookingModel>(SearchAndBookingServiceDataJson) ?? new();

        Schedule goSchedule;
        Schedule? returnSchedule = null!;
        goSchedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
            .Single(s => s.Id == Data.GoSchedule.ScheduleId);
        if (Data.Search.IsRoundtrip)
        {
            returnSchedule = _appDBContext
                .Schedules
                .Include(s => s.Flights)
                .SingleOrDefault(s => s.Id == Data.ReturnSchedule.ScheduleId);
        }



        vv_airline.Models.Data.Price goPrice = _appDBContext
            .Prices
            .Single(p =>
                p.Schedule == goSchedule
                && p.SeatClass.Name == Data.GoSchedule.ClassName
            );

        vv_airline.Models.Data.Price? returnPrice = null!;
        if (Data.ReturnSchedule != null)
            returnPrice = _appDBContext
                .Prices
                .Single(p =>
                    p.Schedule == returnSchedule
                    && p.SeatClass.Name == Data.ReturnSchedule.ClassName
                );

        long totalPrice = (goPrice.Value + (returnPrice?.Value ?? 0))
        * (Data.Search.Adults + Data.Search.Children);



        Booking newBooking = new Booking()
        {
            Adults = Data.Search.Adults,
            Children = Data.Search.Adults,
            IsRoundtrip = Data.Search.IsRoundtrip,
            TotalPrice = totalPrice,
        };
        if (User.Identity.Name != null)
            newBooking.User = _appDBContext.Users.Where(u => u.UserName == User.Identity.Name).Single();

        newBooking.Schedules.Add(goSchedule);
        if (returnSchedule != null)
        {
            Console.WriteLine("here");
            newBooking.Schedules.Add(returnSchedule);
        }

        // Console.WriteLine("---------");
        // Console.WriteLine(newBooking.Schedules.Count);
        // Console.WriteLine("---------");

        List<Ticket> tickets = new() { };

        List<Passenger> adultPassengers = new List<Passenger>();
        for (int i = 0; i < Data.AdultsPassengers.Length; i++)
        {
            var passenger = Data.AdultsPassengers[i];
            Passenger newAdultPassenger = new Passenger()
            {
                Booking = newBooking,
                Type = PassengerEnums.Type.Adult,
                Email = passenger.Email,
                PhoneNumber = passenger.PhoneNumber,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                DateOfBirth = passenger.DateOfBirth,
                Gender = passenger.Gender,

            };
            adultPassengers.Add(newAdultPassenger);
            Ticket? goTicketLeg1;
            Ticket? goTicketLeg2;
            Ticket? returnTicketLeg1;
            Ticket? returnTicketLeg2;
            goTicketLeg1 = new Ticket()
            {
                Flight = goSchedule.Flights[0],
                Passenger = newAdultPassenger,
                Seat = _appDBContext.Seats.Single(s => s.Id == Data.GoSchedule.Leg1.AdultsSeats[i].SeatId),
            };
            tickets.Add(goTicketLeg1);
            if (goSchedule.HasTransit)
            {
                goTicketLeg2 = new Ticket()
                {
                    Flight = goSchedule.Flights[1],
                    Passenger = newAdultPassenger,
                    Seat = _appDBContext.Seats.Single(s => s.Id == Data.GoSchedule.Leg2.AdultsSeats[i].SeatId),
                };
                tickets.Add(goTicketLeg2);
            }
            if (Data.Search.IsRoundtrip)
            {
                returnTicketLeg1 = new Ticket()
                {
                    Flight = returnSchedule.Flights[0],
                    Passenger = newAdultPassenger,
                    Seat = _appDBContext.Seats.Single(s => s.Id == Data.ReturnSchedule.Leg1.AdultsSeats[i].SeatId),
                };
                tickets.Add(returnTicketLeg1);
                if (returnSchedule.HasTransit)
                {
                    returnTicketLeg2 = new Ticket()
                    {
                        Flight = returnSchedule.Flights[1],
                        Passenger = newAdultPassenger,
                        Seat = _appDBContext.Seats.Single(s => s.Id == Data.ReturnSchedule.Leg2.AdultsSeats[i].SeatId),
                    };
                    tickets.Add(returnTicketLeg2);
                }
            }
        }
        List<Passenger> childrenPassengers = new List<Passenger>();
        foreach (var passenger in Data.ChildrenPassengers)
        {
            Passenger newChildPassenger = new Passenger()
            {
                Booking = newBooking,
                Type = PassengerEnums.Type.Child,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                DateOfBirth = passenger.DateOfBirth,
                Gender = passenger.Gender,
            };
            childrenPassengers.Add(newChildPassenger);
        }

        if (newBooking != null) _appDBContext.Add(newBooking);
        if (adultPassengers != null) _appDBContext.AddRange(adultPassengers);
        if (childrenPassengers != null) _appDBContext.AddRange(childrenPassengers);
        if (tickets != null) _appDBContext.AddRange(tickets);

        if (newBooking != null) Console.WriteLine("newBooking");
        if (adultPassengers != null) Console.WriteLine("adultPassengers");
        if (childrenPassengers != null) Console.WriteLine("childrenPassengers");
        if (tickets != null) Console.WriteLine("tickets");

        _appDBContext.SaveChanges();

        return Redirect("/flights");
    }

    // show user flights
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
        // throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("book-flights")]
    public async Task<IActionResult> BookFlights()
    {
        throw new NotImplementedException();
    }
}