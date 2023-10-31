using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vv_airline.Models.Data;

namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class SearchModel
{
    public long DepartureAirportId { get; set; }
     public long DestinationAirportId { get; set; }
     public bool IsRoundtrip { get; set; }
     public byte Adults { get; set; }
     public byte Children { get; set; }
     public DateOnly DepartureDate { get; set; }
     public DateOnly? ReturnDate { get; set; }
}