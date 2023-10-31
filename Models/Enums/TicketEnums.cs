using System.ComponentModel;

namespace vv_airline.Models.Enums;

public static class TicketEnums
{
    public enum Statuses
    {
        [Description("Chờ thanh toán")]
        WaitingForPayment,
        [Description("Đã thanh toán")]
        Paid,
        [Description("Đã hủy")]
        Canceled,
    };
}