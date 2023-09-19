using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[PrimaryKey("ScheduleId", "BookingId")]
[Table("Schedule_Booking")]
[Microsoft.EntityFrameworkCore.Index("BookingId", Name = "IX_Schedule_Booking_booking_id")]
public partial class ScheduleBooking
{
    [Key]
    [Column("schedule_id")]
    public long ScheduleId { get; set; }

    [Key]
    [Column("booking_id")]
    public long BookingId { get; set; }

    [Column("index")]
    public byte Index { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("ScheduleBookings")]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey("ScheduleId")]
    [InverseProperty("ScheduleBookings")]
    public virtual Schedule Schedule { get; set; } = null!;
}
