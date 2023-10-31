using System.ComponentModel;

namespace vv_airline.Models.Enums;

public static class SeatEnums
{
    public enum Classes
    {
        [Description("Hạng thương gia")]
        Business,

        [Description("Hạng Phổ thông")]
        Economy,
    };
    public enum Statuses
    {
        [Description("Còn trống")]
        Available,
        [Description("Không còn trống")]
        Unavailable,
    };
}