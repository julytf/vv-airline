using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vv_airline.Models.Data;

namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class SeatModel
{
    public long SeatId { get; set; }
    public byte Row { get; set; }
    public char Col { get; set; }
}