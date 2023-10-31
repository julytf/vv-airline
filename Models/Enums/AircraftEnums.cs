using System.ComponentModel;

namespace vv_airline.Models.Enums;

public static class AircraftEnums
{
    public enum Statuses
    {
        [Description("Khả dụng")]
        Available,
        [Description("Không khả dụng")]
        Unavailable,
    };
}