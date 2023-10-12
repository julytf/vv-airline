namespace vv_airline.Areas.SearchAndBooking.Models;
public class SearchModel
{
    public long DepartureAirport { get; set; }
    public long DestinationAirport { get; set; }
    public long SeatClass { get; set; }
    public bool IsRoundtrip { get; set; }
    public byte Adults { get; set; }
    public byte Children { get; set; }
    public DateOnly DepartureDate { get; set; }
    public DateOnly ArrivalDate { get; set; }

}