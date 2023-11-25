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

        ScheduleModel scheduleModel = new()
        {
            ScheduleId = scheduleSelectionModel.ScheduleId,
            ClassName = scheduleSelectionModel.ClassName,
            Leg1 = new()
            {
                FlightId = schedule.Flights[0].Id,
                AdultsSeats = new SeatModel[Data.Search.Adults],
                ChildrenSeats = new SeatModel[Data.Search.Children],
            },
            Leg2 = schedule.Flights.Count < 2 ? null : new()
            {
                FlightId = schedule.Flights[1].Id,
                AdultsSeats = new SeatModel[Data.Search.Adults],
                ChildrenSeats = new SeatModel[Data.Search.Children],
            }
        };

        // Console.WriteLine("---");
        // Console.WriteLine(scheduleSelectionModel.Type.ToString());
        // Console.WriteLine("---");
        if (scheduleSelectionModel.Type == ScheduleSelectionModel.ScheduleType.Go)
        {
            Data.GoSchedule = scheduleModel;
            // Console.WriteLine("go");
        }
        else if (scheduleSelectionModel.Type == ScheduleSelectionModel.ScheduleType.Return)
        {
            Data.ReturnSchedule = scheduleModel;
            // Console.WriteLine("return");
        }


        SaveToSession();
    }
    public void Step3PassengersInformation(PassengersInformationModel passengersInformationModel)
    {
        Data.AdultsPassengers = new PassengerModel[Data.Search.Adults];
        Data.ChildrenPassengers = new PassengerModel[Data.Search.Children];

        Data.AdultsPassengers[0] = new()
        {
            Type = PassengerEnums.Type.Adult,
            Email = passengersInformationModel.Adults[0].Email,
            PhoneNumber = passengersInformationModel.Adults[0].PhoneNumber,
            FirstName = passengersInformationModel.Adults[0].FirstName,
            LastName = passengersInformationModel.Adults[0].LastName,
            DateOfBirth = passengersInformationModel.Adults[0].DateOfBirth,
            Gender = passengersInformationModel.Adults[0].Gender,
        };
        for (int i = 0; i < passengersInformationModel.Adults.Length; i++)
        {
            Data.AdultsPassengers[i] = new()
            {
                Type = PassengerEnums.Type.Adult,
                FirstName = passengersInformationModel.Adults[i].FirstName,
                LastName = passengersInformationModel.Adults[i].LastName,
                DateOfBirth = passengersInformationModel.Adults[i].DateOfBirth,
                Gender = passengersInformationModel.Adults[i].Gender,
            };
        }
        for (int i = 0; i < passengersInformationModel.Children.Length; i++)
        {
            Data.ChildrenPassengers[i] = new()
            {
                Type = PassengerEnums.Type.Adult,
                FirstName = passengersInformationModel.Children[i].FirstName,
                LastName = passengersInformationModel.Children[i].LastName,
                DateOfBirth = passengersInformationModel.Children[i].DateOfBirth,
                Gender = passengersInformationModel.Children[i].Gender,
            };
        }

        SaveToSession();
    }
    public void Step4AdditionServicesSelection(AdditionServicesSelectionModel additionServicesSelectionModel) { }

    public void Step5SeatsSelection(SeatSelectionModel seatSelectionModel)
    {
        Seat seat = _appDBContext.Seats.First(s => s.Id == seatSelectionModel.SeatId);

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
        SeatModel[] seats;
        if (seatSelectionModel.PassengerType == PassengerEnums.Type.Adult)
        {
            seats = flightModel.AdultsSeats;
        }
        else
        {
            seats = flightModel.ChildrenSeats;
        }

        seats[seatSelectionModel.PassengerIndex] = new SeatModel()
        {
            SeatId = seatSelectionModel.SeatId,
            Col = seat.Col,
            Row = seat.Row,
        };
        SaveToSession();
    }
    public void Step6Checkout(CheckoutModel checkoutModel) { }
}