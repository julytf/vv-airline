using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("AircraftRegistrationNumber", Name = "IX_Flights_aircraft_registration_number")]
[Index("RouteId", Name = "IX_Flights_route_id")]
public partial class Flight
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("aircraft_registration_number")]
    [StringLength(255)]
    [Unicode(false)]
    public string AircraftRegistrationNumber { get; set; } = null!;

    [Column("departure_time", TypeName = "datetime")]
    public DateTime DepartureTime { get; set; }

    [Column("arrival_time", TypeName = "datetime")]
    public DateTime ArrivalTime { get; set; }

    [Column("route_id")]
    public long RouteId { get; set; }

    [Column("status")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("AircraftRegistrationNumber")]
    [InverseProperty("Flights")]
    public virtual Aircraft AircraftRegistrationNumberNavigation { get; set; } = null!;

    [InverseProperty("Flight")]
    public virtual ICollection<FlightSchedule> FlightSchedules { get; set; } = new List<FlightSchedule>();

    [ForeignKey("RouteId")]
    [InverseProperty("Flights")]
    public virtual Route Route { get; set; } = null!;

    [InverseProperty("Flight")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
