using System.ComponentModel;

namespace vv_airline.Models.Enums;

public static class PassengerEnums
{
    public enum Type
    {
        [Description("Người lớn")]
        Adult,
        [Description("Trẻ em")]
        Child,
    };
}