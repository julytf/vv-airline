using System.ComponentModel;

namespace vv_airline.Models.Enums;

public static class UserEnums
{
    public enum Roles
    {
        [Description("Admin")]
        Admin,
        [Description("Nhân viên")]
        Staff,
        [Description("Khách hàng")]
        User,
    };
}