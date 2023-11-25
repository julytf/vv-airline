using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
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
    private readonly UserManager<User> _userManager;
    public SearchController(
        AppDBContext appDBContext,
        SearchAndBookingService searchAndBookingService,
        UserManager<User> userManager
        )
    {
        _appDBContext = appDBContext;
        _searchAndBookingService = searchAndBookingService;
        _userManager = userManager;
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
        searchModel.Validate(ModelState);
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
        return _SchedulesSelectionView(ScheduleSelectionModel.ScheduleType.Go);
    }

    [HttpPost("schedules-selection")]
    public IActionResult SchedulesSelection(
        ScheduleSelectionModel scheduleSelectionModel,
         string? Action
        )
    {
        if (Action == "toGo")
        {
            return _SchedulesSelectionView(ScheduleSelectionModel.ScheduleType.Go);
        }
        if (Action == "toReturn")
        {
            return _SchedulesSelectionView(ScheduleSelectionModel.ScheduleType.Return);
        }

        _searchAndBookingService.Step2SchedulesSelection(scheduleSelectionModel);

        // TODO:
        ModelState.Remove("Type");

        if (_searchAndBookingService.Data.GoSchedule == null)
        {
            return _SchedulesSelectionView(ScheduleSelectionModel.ScheduleType.Go);
        }
        if (_searchAndBookingService.Data.Search.IsRoundtrip && _searchAndBookingService.Data.ReturnSchedule == null)
        {
            return _SchedulesSelectionView(ScheduleSelectionModel.ScheduleType.Return);
        }

        return RedirectToAction("PassengersInformation");
    }


    private IActionResult _SchedulesSelectionView(ScheduleSelectionModel.ScheduleType type)
    {
        ScheduleSelectionModel model = new()
        {
            Type = type
        };
        FlightRoute flightRoute = new();
        if (type == ScheduleSelectionModel.ScheduleType.Go)
        {
            model.Type = ScheduleSelectionModel.ScheduleType.Go;
            flightRoute = _appDBContext
                .FlightRoutes
                .SingleOrDefault(r =>
                    r.DepartureAirport.Id == _searchAndBookingService.Data.Search.DepartureAirportId &&
                    r.DestinationAirport.Id == _searchAndBookingService.Data.Search.DestinationAirportId
                );
        }
        else if (type == ScheduleSelectionModel.ScheduleType.Return)
        {
            model.Type = ScheduleSelectionModel.ScheduleType.Return;
            flightRoute = _appDBContext
                .FlightRoutes
                .SingleOrDefault(r =>
                    r.DepartureAirport.Id == _searchAndBookingService.Data.Search.DestinationAirportId &&
                    r.DestinationAirport.Id == _searchAndBookingService.Data.Search.DepartureAirportId
                );
        }

        List<Schedule> schedules = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
            .Include(s => s.FlightRoute)
                .ThenInclude(f => f.DepartureAirport)
            .Include(s => s.FlightRoute)
                .ThenInclude(f => f.DestinationAirport)
            .Include(s => s.Prices)
                .ThenInclude(p => p.SeatClass)
            .Where(s =>
                s.FlightRoute == flightRoute
                && s.DepartureTime.Date == _searchAndBookingService.Data.Search.DepartureDate
            )
            .ToList() ?? new List<Schedule>();
        // Console.WriteLine(JsonConvert.SerializeObject(schedules,
        //     Formatting.Indented,
        //     new JsonSerializerSettings
        //     {
        //         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //     }
        // ));
        // Price economyPrice = _appDBContext
        //     .Prices
        //     .Single(p =>
        //         p.Schedule == goSchedule
        //         && p.SeatClass.Name == _searchAndBookingService.Data.GoSchedule.ClassName
        //     );

        // Price businessPrice = _appDBContext
        //     .Prices
        //     .Single(p =>
        //         p.Schedule == returnSchedule
        //         && p.SeatClass.Name == _searchAndBookingService.Data.ReturnSchedule.ClassName
        //     );

        ViewBag.search = _searchAndBookingService.Data.Search;
        ViewBag.schedules = schedules;
        return View(model);

    }

    [HttpGet("passengers-information")]
    public async Task<IActionResult> PassengersInformation()
    {

        User user = await _userManager.FindByNameAsync(User.Identity.Name);
        PassengersInformationModel model = new();
        model.Adults = new PassengersInformationModel.PassengerInformationModel[_searchAndBookingService.Data.Search.Adults];
        model.Adults[0] = new()
        {
            Type = PassengerEnums.Type.Adult,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
        };
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
        return _SeatsSelectionView(
            new SeatSelectionModel() { },
            SeatSelectionModel.TurnEnum.Go,
            SeatSelectionModel.LegEnum.One
            );
    }

    [HttpPost("seats-selection")]
    public IActionResult SeatsSelection(
        SeatSelectionModel seatSelectionModel,
         string? Action
        )
    {
        if (Action == "next")
        {
            return RedirectToAction("Checkout");
        }
        else if (Action == "toGoLeg1")
        {
            return _SeatsSelectionView(
                seatSelectionModel,
                SeatSelectionModel.TurnEnum.Go,
                SeatSelectionModel.LegEnum.One
                );
        }
        else if (Action == "toGoLeg2")
        {
            return _SeatsSelectionView(
                seatSelectionModel,
                SeatSelectionModel.TurnEnum.Go,
                SeatSelectionModel.LegEnum.Two
                );
        }
        else if (Action == "toReturnLeg1")
        {
            return _SeatsSelectionView(
                seatSelectionModel,
                SeatSelectionModel.TurnEnum.Return,
                SeatSelectionModel.LegEnum.One
                );
        }
        else if (Action == "toReturnLeg2")
        {
            return _SeatsSelectionView(
                seatSelectionModel,
                SeatSelectionModel.TurnEnum.Return,
                SeatSelectionModel.LegEnum.Two
                );
        }

        _searchAndBookingService.Step5SeatsSelection(seatSelectionModel);


        return _SeatsSelectionView(
            seatSelectionModel,
            seatSelectionModel.Turn,
            seatSelectionModel.Leg
            );
    }

    public IActionResult _SeatsSelectionView(SeatSelectionModel seatSelectionModel, SeatSelectionModel.TurnEnum turn, SeatSelectionModel.LegEnum leg)
    {
        long scheduleId = 0;
        if (turn == SeatSelectionModel.TurnEnum.Go)
        {
            scheduleId = _searchAndBookingService.Data.GoSchedule.ScheduleId ?? 0;
        }
        else if (turn == SeatSelectionModel.TurnEnum.Return)
        {
            scheduleId = _searchAndBookingService.Data.ReturnSchedule.ScheduleId ?? 0;
        }
        var schedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(f => f.Aircraft)
                    .ThenInclude(ac => ac.Model)
            .Single(
                s => s.Id == scheduleId
            );

        Aircraft aircraft = null!;

        if (leg == SeatSelectionModel.LegEnum.One)
        {
            aircraft = schedule.Flights[0].Aircraft;
        }
        else if (leg == SeatSelectionModel.LegEnum.Two)
        {
            aircraft = schedule.Flights[1].Aircraft;
        }
        var seatMap = _appDBContext
            .SeatMaps
            .Include(sm => sm.AisleCols)
            .Include(sm => sm.ExitRows)
            .Include(sm => sm.Seats)
            .First(sm => sm.Model == aircraft.Model);

        // seatSelectionModel = new()
        // {
        //     Turn = SeatSelectionModel.TurnEnum.Go,
        //     Leg = SeatSelectionModel.LegEnum.One,
        //     PassengerType = PassengerEnums.Type.Adult,
        //     PassengerIndex = 0,
        // };
        dynamic? adultSeats = null!;
        dynamic? childrenSeats = null!;
        dynamic? scheduleModel = null!;
        dynamic? legModel = null!;

        if (turn == SeatSelectionModel.TurnEnum.Go)
        {
            scheduleModel = _searchAndBookingService.Data.GoSchedule;
        }
        else if (turn == SeatSelectionModel.TurnEnum.Return)
        {
            scheduleModel = _searchAndBookingService.Data.ReturnSchedule;
        }
        if (leg == SeatSelectionModel.LegEnum.One)
        {
            legModel = scheduleModel.Leg1;
        }
        else if (leg == SeatSelectionModel.LegEnum.Two)
        {
            legModel = scheduleModel.Leg2;
        }

        adultSeats = legModel.AdultsSeats;
        childrenSeats = legModel.ChildrenSeats;

        // Console.WriteLine(_searchAndBookingService.Data.GoSchedule.ScheduleId);
        // Console.WriteLine(_searchAndBookingService.Data.ReturnSchedule?.ScheduleId);
        var goSchedule = _appDBContext
                    .Schedules
                    .Include(s => s.Flights)
                        .ThenInclude(f => f.FlightRoute)
                            .ThenInclude(fr => fr.DepartureAirport)
                    .Include(s => s.Flights)
                        .ThenInclude(f => f.FlightRoute)
                            .ThenInclude(fr => fr.DestinationAirport)
                    .Single(s => s.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId);
        Schedule? ReturnSchedule = null;
        if (_searchAndBookingService.Data.ReturnSchedule != null)
        {
            ReturnSchedule = _appDBContext
                        .Schedules
                        .Include(s => s.Flights)
                            .ThenInclude(f => f.FlightRoute)
                                .ThenInclude(fr => fr.DepartureAirport)
                        .Include(s => s.Flights)
                            .ThenInclude(f => f.FlightRoute)
                                .ThenInclude(fr => fr.DestinationAirport)
                        .SingleOrDefault(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.ScheduleId);
        }
        ModelState.Remove("Turn");
        ModelState.Remove("Leg");
        seatSelectionModel.Turn = turn;
        seatSelectionModel.Leg = leg;

        ViewBag.search = _searchAndBookingService.Data.Search;
        ViewBag.adultPassengers = _searchAndBookingService.Data.AdultsPassengers;
        ViewBag.childrenPassengers = _searchAndBookingService.Data.ChildrenPassengers;
        ViewBag.adultSeats = adultSeats;
        ViewBag.childrenSeats = childrenSeats;
        ViewBag.seatMap = seatMap;
        ViewBag.goSchedule = goSchedule;
        ViewBag.ReturnSchedule = ReturnSchedule;

        return View(seatSelectionModel);
    }


    [HttpGet("checkout")]
    public IActionResult Checkout()
    {
        var goSchedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
                .ThenInclude(s => s.Aircraft)
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
            .Include(s => s.Flights)
                .ThenInclude(f => f.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
            .Include(s => s.FlightRoute)
                .ThenInclude(fr => fr.DepartureAirport)
            .Include(s => s.FlightRoute)
                .ThenInclude(fr => fr.DestinationAirport)
            .First(fr => fr.Id == _searchAndBookingService.Data.GoSchedule.ScheduleId);
        Schedule returnSchedule = null!;
        if (_searchAndBookingService.Data.ReturnSchedule != null)
        {
            returnSchedule = _appDBContext
                .Schedules
                .Include(s => s.Flights)
                    .ThenInclude(s => s.Aircraft)
                .Include(s => s.Flights)
                    .ThenInclude(f => f.FlightRoute)
                        .ThenInclude(fr => fr.DepartureAirport)
                .Include(s => s.Flights)
                    .ThenInclude(f => f.FlightRoute)
                        .ThenInclude(fr => fr.DestinationAirport)
                .Include(s => s.FlightRoute)
                    .ThenInclude(fr => fr.DepartureAirport)
                .Include(s => s.FlightRoute)
                    .ThenInclude(fr => fr.DestinationAirport)
                .FirstOrDefault(fr => fr.Id == _searchAndBookingService.Data.ReturnSchedule.ScheduleId);
        }
        Price goPrice = _appDBContext
            .Prices
            .Single(p =>
                p.Schedule == goSchedule
                && p.SeatClass.Name == _searchAndBookingService.Data.GoSchedule.ClassName
            );

        Price returnPrice = null!;
        if (_searchAndBookingService.Data.ReturnSchedule != null)
        {
            returnPrice = _appDBContext
            .Prices
            .Single(p =>
                p.Schedule == returnSchedule
                && p.SeatClass.Name == _searchAndBookingService.Data.ReturnSchedule.ClassName
            );
        }
        int passengersCount = _searchAndBookingService.Data.Search.Adults + _searchAndBookingService.Data.Search.Children;
        long totalPrice =
            passengersCount * (
                goPrice.Value
                + (returnPrice?.Value ?? 0)
            );

        ViewBag.data = _searchAndBookingService.Data;
        ViewBag.goSchedule = goSchedule;
        ViewBag.returnSchedule = returnSchedule;
        ViewBag.passengersCount = _searchAndBookingService.Data.Search.Adults + _searchAndBookingService.Data.Search.Children;
        ViewBag.goPrice = goPrice.Value;
        ViewBag.returnPrice = returnPrice?.Value;
        ViewBag.totalPrice = totalPrice;
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
        if (_searchAndBookingService.Data.Search.IsRoundtrip)
        {
            returnSchedule = _appDBContext
                .Schedules
                .Include(s => s.Flights)
                .SingleOrDefault(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.ScheduleId);
        }



        Price goPrice = _appDBContext
            .Prices
            .Single(p =>
                p.Schedule == goSchedule
                && p.SeatClass.Name == _searchAndBookingService.Data.GoSchedule.ClassName
            );

        Price returnPrice = _appDBContext
            .Prices
            .Single(p =>
                p.Schedule == returnSchedule
                && p.SeatClass.Name == _searchAndBookingService.Data.ReturnSchedule.ClassName
            );


        Booking newBooking = new Booking()
        {
            Adults = _searchAndBookingService.Data.Search.Adults,
            Children = _searchAndBookingService.Data.Search.Adults,
            IsRoundtrip = _searchAndBookingService.Data.Search.IsRoundtrip,
            TotalPrice = (goPrice.Value + returnPrice.Value)
            * (_searchAndBookingService.Data.Search.Adults + _searchAndBookingService.Data.Search.Children),
            User = _appDBContext.Users.Where(u => u.UserName == User.Identity.Name).Single()
        };

        newBooking.Schedules.Add(goSchedule);
        if (returnSchedule != null)
        {
        Console.WriteLine("here");
            newBooking.Schedules.Add(returnSchedule);
        }

        Console.WriteLine("---------");
        Console.WriteLine(newBooking.Schedules.Count);
        Console.WriteLine("---------");

        List<Ticket> tickets = new() { };

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
            tickets.Add(goTicketLeg1);
            if (goSchedule.HasTransit)
            {
                goTicketLeg2 = new Ticket()
                {
                    Flight = goSchedule.Flights[1],
                    Passenger = newAdultPassenger,
                    Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.GoSchedule.Leg2.AdultsSeats[i].SeatId),
                };
                tickets.Add(goTicketLeg2);
            }
            if (_searchAndBookingService.Data.Search.IsRoundtrip)
            {
                returnTicketLeg1 = new Ticket()
                {
                    Flight = returnSchedule.Flights[0],
                    Passenger = newAdultPassenger,
                    Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.Leg1.AdultsSeats[i].SeatId),
                };
                tickets.Add(returnTicketLeg1);
                if (returnSchedule.HasTransit)
                {
                    returnTicketLeg2 = new Ticket()
                    {
                        Flight = returnSchedule.Flights[1],
                        Passenger = newAdultPassenger,
                        Seat = _appDBContext.Seats.Single(s => s.Id == _searchAndBookingService.Data.ReturnSchedule.Leg2.AdultsSeats[i].SeatId),
                    };
                    tickets.Add(returnTicketLeg2);
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

        if (newBooking != null) _appDBContext.Add(newBooking);
        if (adultPassengers != null) _appDBContext.AddRange(adultPassengers);
        if (childrenPassengers != null) _appDBContext.AddRange(childrenPassengers);
        if (tickets != null) _appDBContext.AddRange(tickets);

        if (newBooking != null) Console.WriteLine("newBooking");
        if (adultPassengers != null) Console.WriteLine("adultPassengers");
        if (childrenPassengers != null) Console.WriteLine("childrenPassengers");
        if (tickets != null) Console.WriteLine("tickets");

        _appDBContext.SaveChanges();

        return Redirect("/");
    }
}
