using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[PrimaryKey("FlightId", "ScheduleId")]
[Table("Flight_Schedule")]
[Microsoft.EntityFrameworkCore.Index("ScheduleId", Name = "IX_Flight_Schedule_schedule_id")]
public partial class FlightSchedule
{
    [Key]
    [Column("flight_id")]
    public long FlightId { get; set; }

    [Key]
    [Column("schedule_id")]
    public long ScheduleId { get; set; }

    [Column("index")]
    public int Index { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("FlightSchedules")]
    public virtual Flight Flight { get; set; } = null!;

    [ForeignKey("ScheduleId")]
    [InverseProperty("FlightSchedules")]
    public virtual Schedule Schedule { get; set; } = null!;
}
