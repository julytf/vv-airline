using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("UserId", Name = "IX_Bookings_user_id")]
public partial class Booking
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public string? UserId { get; set; }

    [Column("adults")]
    public byte Adults { get; set; }

    [Column("children")]
    public byte? Children { get; set; }

    [Column("is_roundtrip")]
    public bool? IsRoundtrip { get; set; }

    [Column("total_price", TypeName = "decimal(18, 0)")]
    public decimal TotalPrice { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    [InverseProperty("Booking")]
    public virtual ICollection<ScheduleBooking> ScheduleBookings { get; set; } = new List<ScheduleBooking>();

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User? User { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("Bookings")]
    public virtual ICollection<Service> ServiceNames { get; set; } = new List<Service>();
}
