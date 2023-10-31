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
    public async Task<IActionResult> Index()
    {
        List<Airport> airports = _appDBContext.Airports.ToList();
        ViewBag.airports = airports;
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(SearchModel searchModel)
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
    public async Task<IActionResult> SchedulesSelection()
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
    public async Task<IActionResult> SchedulesSelection(
        ScheduleSelectionModel scheduleSelectionModel
        )
    {
        _searchAndBookingService.Step2SchedulesSelection(scheduleSelectionModel);
        return RedirectToAction("PassengersInformation");
    }
    
    [HttpGet("passengers-information")]
    public async Task<IActionResult> PassengersInformation()
    {
        PassengersInformationModel model = new();
        model.Adults = new PassengersInformationModel.PassengerInformationModel[_searchAndBookingService.Data.Search.Adults];
        model.Children = new PassengersInformationModel.PassengerInformationModel[_searchAndBookingService.Data.Search.Children];
        return View(model);
    }
    
    [HttpPost("passengers-information")]
    public async Task<IActionResult> PassengersInformation(PassengersInformationModel passengersInformationModel)
    {
        
        if (!ModelState.IsValid)
        {
            return View(passengersInformationModel);
        }

        _searchAndBookingService.Step3PassengersInformation(passengersInformationModel);
        return RedirectToAction("AdditionServicesSelection");
    }
        
    
    [HttpGet("addition-services-selection")]
    public async Task<IActionResult> AdditionServicesSelection()
    {
        // throw new NotImplementedException();
        // TODO:
        return RedirectToAction("SeatsSelection");
    }
    
    [HttpGet("seats-selection")]
    public async Task<IActionResult> SeatsSelection()
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

        ViewBag.seatMap = seatMap;
        return View();
    }
    
    [HttpPost("seats-selection")]
    public async Task<IActionResult> SeatsSelection(SeatSelectionModel seatSelectionModel)
    {
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

        ViewBag.seatMap = seatMap;
        return View();
    }
    
    [HttpGet("checkout")]
    public async Task<IActionResult> Checkout()
    {
        throw new NotImplementedException();
    }
}
