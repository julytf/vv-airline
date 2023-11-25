namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class ScheduleModel
{
    public long? ScheduleId { get; set; }
    public string? ClassName { get; set; }

    public FlightModel Leg1 { get; set; } = new();
    public FlightModel? Leg2 { get; set; } = new();
}