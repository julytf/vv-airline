using vv_airline.Models.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
using vv_airline.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Areas.SearchAndBooking.Models.Services;
public class SearchAndBookingService
{
    private ISession _session;
    private SearchEnums.Step _currentStep;
    public SearchAndBookingModel Data;
    private readonly AppDBContext _appDBContext;
    public SearchAndBookingService(
        AppDBContext appDBContext,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _appDBContext = appDBContext;
        _session = httpContextAccessor.HttpContext.Session;

        LoadFromSession();
    }

    public void ResetSession()
    {
        throw new NotImplementedException();
    }
    public void SaveToSession()
    {
        string SearchAndBookingServiceDataJson = JsonConvert.SerializeObject(Data);
        _session.SetString("SearchAndBookingServiceDataJson", SearchAndBookingServiceDataJson);
        Console.WriteLine(SearchAndBookingServiceDataJson);
    }
    public void LoadFromSession()
    {
        string SearchAndBookingServiceDataJson = _session.GetString("SearchAndBookingServiceDataJson") ?? "";
        Data = JsonConvert.DeserializeObject<SearchAndBookingModel>(SearchAndBookingServiceDataJson) ?? new();
    }
    public void Step1Search(SearchModel searchModel)
    {
        Data.Search = new()
        {
            DepartureAirportId = searchModel.DepartureAirportId,
            DestinationAirportId = searchModel.DestinationAirportId,
            IsRoundtrip = searchModel.IsRoundtrip,
            Adults = searchModel.Adults,
            Children = searchModel.Children,
            DepartureDate = searchModel.DepartureDate,
            ReturnDate = searchModel.ReturnDate,
        };
        SaveToSession();
    }
    public void Step2SchedulesSelection(ScheduleSelectionModel scheduleSelectionModel)
    {
        Schedule schedule = _appDBContext
            .Schedules
            .Include(s => s.Flights)
            .First(s => s.Id == scheduleSelectionModel.ScheduleId);
        // Console.WriteLine(schedule.Id);
        // Console.WriteLine(schedule.Flights[0].Id);
        Data.GoSchedule = new()
        {
            ScheduleId = scheduleSelectionModel.ScheduleId,
            ClassName = scheduleSelectionModel.ClassName,
            Leg1 = new FlightModel()
            {
                FlightId = schedule.Flights[0].Id
            }
        };
        // Console.WriteLine(Data.GoSchedule.Leg1.FlightId);
        SaveToSession();
    }
    public void Step3PassengersInformation(PassengersInformationModel passengersInformationModel)
    {
        Data.AdultsPassengers = new[]{
            new PassengerModel() {
                Type = passengersInformationModel.Adults[0].Type,
                Email = passengersInformationModel.Adults[0].Email,
                PhoneNumber = passengersInformationModel.Adults[0].PhoneNumber,
                FirstName = passengersInformationModel.Adults[0].FirstName,
                LastName = passengersInformationModel.Adults[0].LastName,
                DateOfBirth = passengersInformationModel.Adults[0].DateOfBirth,
                Gender = passengersInformationModel.Adults[0].Gender,
            }
        };
        SaveToSession();
    }
    public void Step4AdditionServicesSelection(AdditionServicesSelectionModel additionServicesSelectionModel) { }
    public void Step5SeatsSelection(SeatSelectionModel seatSelectionModel)
    {
        ScheduleModel scheduleModel;
        if (seatSelectionModel.Turn == SeatSelectionModel.TurnEnum.Go)
        {
            scheduleModel = Data.GoSchedule;
        }
        else
        {
            scheduleModel = Data.ReturnSchedule;
        }
        FlightModel flightModel;
        if (seatSelectionModel.Leg == SeatSelectionModel.LegEnum.One)
        {
            flightModel = scheduleModel.Leg1;
        }
        else
        {
            flightModel = scheduleModel.Leg2;
        }
        List<SeatModel> seatModels;
        if (seatSelectionModel.PassengerType == PassengerEnums.Type.Adult)
        {
            seatModels = flightModel.AdultsSeats;
        }
        else
        {
            seatModels = flightModel.ChildrenSeats;
        }
        seatModels[seatSelectionModel.PassengerIndex] =
            new SeatModel()
            {
                SeatId = seatSelectionModel.SeatId,
            }
        ;
        SaveToSession();
    }
    public void Step6Checkout(CheckoutModel checkoutModel) { }
}