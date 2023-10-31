using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class SearchAndBookingModel
{
    public SearchModel? Search { get; set; }
    public ScheduleModel? GoSchedule { get; set; }
    public ScheduleModel? ReturnSchedule { get; set; }
    public searchAndBookingService.PassengerModel[]? AdultsPassengers { get; set; }
    public searchAndBookingService.PassengerModel[]? ChildrenPassengers { get; set; }

}