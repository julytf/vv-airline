using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vv_airline.Areas.SearchAndBooking.Models;
using vv_airline.Areas.SearchAndBooking.Models.Services;
using vv_airline.Controllers;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Areas.SearchAndBooking.Controllers;
[Area("SearchAndBooking")]
[Route("/search")]
public class SearchController : AppBaseController
{
    private readonly AppDBContext _appDBContext;
    private readonly SearchAndBookingService _searchAndBookingService;
    public SearchController(
        AppDBContext appDBContext,
        SearchAndBookingService searchAndBookingService
        )
    {
        _appDBContext = appDBContext;
        _searchAndBookingService = searchAndBookingService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Airport> airports = _appDBContext.Airports.ToList();
        ViewBag.airports = airports;
        return View();
    }

    [HttpPost]
    public IActionResult Index(SearchModel searchModel)
    {
        if (!ModelState.IsValid)
        {
            List<Airport> airports = _appDBContext.Airports.ToList();
            ViewBag.airports = airports;
            return View(searchModel);
        }
        _searchAndBookingService.Step1Search(searchModel);
        return RedirectToAction("SchedulesSelection");
    }


    [HttpGet("schedules-selection")]
    public IActionResult SchedulesSelection()
    {
        FlightRoute flightRoute = _appDBContext
            .FlightRoutes
            .Where(r =>
                r.DepartureAirport.Id == _searchAndBookingService.Data.Search.DepartureAirportId &&
                r.DestinationAirport.Id == _searchAndBookingService.Data.Search.DestinationAirportId
            )
            .FirstOrDefault();

        List<Schedule> schedules = _appDBContext
            .Schedules
            .Include(s => s.Flights)
            .Include(s => s.FlightRoute)
                .ThenInclude(f => f.DepartureAirport)
            .Include(s => s.FlightRoute)
                .ThenInclude(f => f.DestinationAirport)
            .Where(s =>
                s.FlightRoute.Id == flightRoute.Id &&
                s.DepartureTime.Date == _searchAndBookingService.Data.Search.DepartureDate.ToDateTime(TimeOnly.MinValue)
            )
            .ToList() ?? new List<Schedule>();
        foreach (Schedule schedule in schedules)
        {
            schedule.Prices[SeatEnums.Classes.Economy.ToString()] = _appDBContext.Prices.Where(p =>
                    p.SeatClass.Name == SeatEnums.Classes.Economy.ToString() &&
                    p.FlightRoute.Id == schedule.FlightRoute.Id
                ).FirstOrDefault().Value;
            schedule.Prices[SeatEnums.Classes.Business.ToString()] = _appDBContext.Prices.Where(p =>
                    p.SeatClass.Name == SeatEnums.Classes.Business.ToString() &&
                    p.FlightRoute.Id == schedule.FlightRoute.Id
                ).FirstOrDefault().Value;
        }

        ViewBag.schedules = schedules;
        return View();
    }

    [HttpPost("schedules-selection")]
    public IActionResult SchedulesSelection(
        ScheduleSelectionModel scheduleSelectionModel
        )
    {
        _searchAndBookingService.Step2SchedulesSelection(scheduleSelectionModel);
        return RedirectToAction("PassengersInformation");
    }

    [HttpGet("passengers-information")]
    public IActionResult PassengersInformation()
    {
        PassengersInformationModel model = new();
        model.Adults = new PassengersInformationModel.PassengerInformationModel[_searchAndBookingService.Data.Search.Adults];
        model.Children = new PassengersInformationModel.PassengerInformationModel[_searchAndBookingService.Data.Search.Children];
        return View(model);
    }

    [HttpPost("passengers-information")]
    public IActionResult PassengersInformation(PassengersInformationModel passengersInformationModel)
    {

        if (!ModelState.IsValid)
        {
            return View(passengersInformationModel);
        }

        _searchAndBookingService.Step3PassengersInformation(passengersInformationModel);
        return RedirectToAction("AdditionServicesSelection");
    }


    [HttpGet("addition-services-selection")]
    public IActionResult AdditionServicesSelection()
    {
        // throw new NotImplementedException();
        // TODO:
        return RedirectToAction("SeatsSelection");
    }

    [HttpGet("seats-selection")]
    public IActionResult SeatsSelection()
    {
        var schedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.Aircraft)
            .First(
                s => s.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId
            );
        // .First();

        var aircraft = schedule.Flights[0].Aircraft;
        var seatMap = _appDBContext
            .SeatMaps
            .Include(sm => sm.AisleCols)
            .Include(sm => sm.ExitRows)
            .Include(sm => sm.Seats)
            .First();

        SeatSelectionModel seatSelectionModel = new()
        {
            Turn = SeatSelectionModel.TurnEnum.Go,
            Leg = SeatSelectionModel.LegEnum.One,
            PassengerType = PassengerEnums.Type.Adult,
            PassengerIndex = 0,
        };

        ViewBag.passengers = _searchAndBookingService.Data.AdultsPassengers;
        ViewBag.seats = _searchAndBookingService.Data.GoSchedule.Leg1.AdultsSeats;
        ViewBag.seatMap = seatMap;
        return View(seatSelectionModel);
    }

    [HttpPost("seats-selection")]
    public IActionResult SeatsSelection(SeatSelectionModel seatSelectionModel, string? Action = null)
    {
        if (Action == "next")
        {
            return RedirectToAction("Checkout");
        }

        _searchAndBookingService.Step5SeatsSelection(seatSelectionModel);

        var schedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.Aircraft)
            .First(
                s => s.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId
            );
        // .First();

        var aircraft = schedule.Flights[0].Aircraft;
        var seatMap = _appDBContext
            .SeatMaps
            .Include(sm => sm.AisleCols)
            .Include(sm => sm.ExitRows)
            .Include(sm => sm.Seats)
            .First();

        ViewBag.passengers = _searchAndBookingService.Data.AdultsPassengers;
        ViewBag.seats = _searchAndBookingService.Data.GoSchedule.Leg1.AdultsSeats;
        ViewBag.seatMap = seatMap;
        return View(seatSelectionModel);
    }

    [HttpGet("checkout")]
    public IActionResult Checkout()
    {
        ViewBag.Data = _searchAndBookingService.Data;
        ViewBag.GoSchedule = _appDBContext
            .Schedules
            .Include(s => s.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(s => s.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .First(fr => fr.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId);
        return View();
    }
    [HttpGet("stripe-webhook")]
    [HttpGet("checkout-success")]
    public IActionResult CheckoutSuccess()
    {
        Schedule goSchedule;
        Schedule? returnSchedule = null!;
        goSchedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
            .Single(s => s.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId);
        // Console.WriteLine("-----------");
        // Console.WriteLine(goSchedule.Id);
        // Console.WriteLine(goSchedule.Flights[0].Id);
        // Console.WriteLine(goSchedule.Flights[0].FlightRoute.Id);
        // Console.WriteLine("-----------");
        if (_searchAndBookingService.Data.Search.IsRoundtrip)
        {
            returnSchedule = _appDBContext
                .Schedules
                .Include(s => s.Flights)
                .SingleOrDefault(s => s.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId);
        }
        Console.WriteLine("-----------");
        Console.WriteLine(goSchedule.Flights[0].FlightRoute.Id);
        Console.WriteLine(_searchAndBookingService.Data.GoSchedule.ClassName);
        Console.WriteLine("-----------");
        long goPrice =
            _appDBContext.Prices.First(p =>
                p.FlightRoute.Id == goSchedule.Flights[0].FlightRoute.Id
                && p.SeatClass.Name == _searchAndBookingService.Data.GoSchedule.ClassName
            ).Value
            + (
            !goSchedule.HasTransit ? 0 :
            _appDBContext.Prices.SingleOrDefault(p =>
                p.FlightRoute.Id == goSchedule.Flights[1].FlightRoute.Id
                && p.SeatClass.Name == _searchAndBookingService.Data.GoSchedule.ClassName
                )?.Value ?? 0
            );
        long returnPrice =
            !_searchAndBookingService.Data.Search.IsRoundtrip ? 0 :
            (_appDBContext.Prices.SingleOrDefault(p =>
                p.FlightRoute.Id == returnSchedule.Flights[0].FlightRoute.Id
                && p.SeatClass.Name == _searchAndBookingService.Data.ReturnSchedule.ClassName
            )?.Value ?? 0
            + (
            !(returnSchedule?.HasTransit ?? false) ? 0 :
            _appDBContext.Prices.SingleOrDefault(p =>
                p.FlightRoute.Id == returnSchedule.Flights[1].FlightRoute.Id
                && p.SeatClass.Name == _searchAndBookingService.Data.ReturnSchedule.ClassName
                )?.Value ?? 0
            ));

        Booking newBooking = new Booking()
        {
            Adults = _searchAndBookingService.Data.Search.Adults,
            Children = _searchAndBookingService.Data.Search.Adults,
            IsRoundtrip = _searchAndBookingService.Data.Search.IsRoundtrip,
            TotalPrice = (goPrice + returnPrice) * (_searchAndBookingService.Data.Search.Adults + _searchAndBookingService.Data.Search.Children),
        };
        List<Passenger> adultPassengers = new List<Passenger>();
        for (int i = 0; i < _searchAndBookingService.Data.AdultsPassengers.Length; i++)
        {
            var passenger = _searchAndBookingService.Data.AdultsPassengers[i];
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
                Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.GoSchedule.Leg1.AdultsSeats[i].SeatId),
            };
            if (goSchedule.HasTransit)
            {
                goTicketLeg2 = new Ticket()
                {
                    Flight = goSchedule.Flights[1],
                    Passenger = newAdultPassenger,
                    Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.GoSchedule.Leg2.AdultsSeats[i].SeatId),
                };
            }
            if (_searchAndBookingService.Data.Search.IsRoundtrip)
            {
                returnTicketLeg1 = new Ticket()
                {
                    Flight = returnSchedule.Flights[0],
                    Passenger = newAdultPassenger,
                    Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.Leg1.AdultsSeats[i].SeatId),
                };
                if (returnSchedule.HasTransit)
                {
                    returnTicketLeg2 = new Ticket()
                    {
                        Flight = returnSchedule.Flights[1],
                        Passenger = newAdultPassenger,
                        Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.Leg2.AdultsSeats[i].SeatId),
                    };
                }
            }
        }
        List<Passenger> childrenPassengers = new List<Passenger>();
        foreach (var passenger in _searchAndBookingService.Data.ChildrenPassengers)
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

        return Redirect("/");
    }
}
