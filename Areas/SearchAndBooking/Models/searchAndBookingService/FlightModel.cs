namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class FlightModel
{
    public long FlightId { get; set; }

    public SeatModel[] AdultsSeats { get; set; } = new SeatModel[0];
    public SeatModel[] ChildrenSeats { get; set; } = new SeatModel[0];
}