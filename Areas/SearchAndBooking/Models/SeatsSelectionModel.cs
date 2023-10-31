using vv_airline.Models.Enums;

namespace vv_airline.Areas.SearchAndBooking.Models;
public class SeatSelectionModel
{
    public enum TurnEnum
    {
        Go,
        Return,
    }
    public enum LegEnum
    {
        One,
        Two,
    }
    public TurnEnum Turn { set; get; }
    public LegEnum Leg { set; get; }
    public PassengerEnums.Type PassengerType { set; get; }
    public int PassengerIndex { set; get; }
    public long SeatId { set; get; }

}