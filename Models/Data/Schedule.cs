using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("RouteId", Name = "IX_Schedules_route_id")]
public partial class Schedule
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("has_transit")]
    public bool? HasTransit { get; set; }

    [Column("route_id")]
    public long RouteId { get; set; }

    [Column("departure_time", TypeName = "datetime")]
    public DateTime DepartureTime { get; set; }

    [Column("arrival_time", TypeName = "datetime")]
    public DateTime ArrivalTime { get; set; }

    [InverseProperty("Schedule")]
    public virtual ICollection<FlightSchedule> FlightSchedules { get; set; } = new List<FlightSchedule>();

    [ForeignKey("RouteId")]
    [InverseProperty("Schedules")]
    public virtual Route Route { get; set; } = null!;

    [InverseProperty("Schedule")]
    public virtual ICollection<ScheduleBooking> ScheduleBookings { get; set; } = new List<ScheduleBooking>();
}
