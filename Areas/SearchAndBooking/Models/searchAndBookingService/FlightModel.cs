namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class FlightModel
{
    public long FlightId { get; set; }

    public List<SeatModel> AdultsSeats { get; set; } = new();
    public List<SeatModel> ChildrenSeats { get; set; } = new();
}