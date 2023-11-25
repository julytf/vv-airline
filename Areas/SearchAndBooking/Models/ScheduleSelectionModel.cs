namespace vv_airline.Areas.SearchAndBooking.Models;
public class ScheduleSelectionModel
{
    public enum ScheduleType
    {
        Go, Return
    };
    public ScheduleType? Type { get; set; }
    public long? ScheduleId { get; set; }
    public string? ClassName { get; set; }
}