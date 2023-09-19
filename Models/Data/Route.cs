using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("DepartureAirport", Name = "IX_Routes_departure_airport")]
[Index("DestinationAirport", Name = "IX_Routes_destination_airport")]
public partial class Route
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("departure_airport")]
    public long DepartureAirport { get; set; }

    [Column("destination_airport")]
    public long DestinationAirport { get; set; }

    [Column("distance")]
    public long? Distance { get; set; }

    [ForeignKey("DepartureAirport")]
    [InverseProperty("RouteDepartureAirportNavigations")]
    public virtual Airport DepartureAirportNavigation { get; set; } = null!;

    [ForeignKey("DestinationAirport")]
    [InverseProperty("RouteDestinationAirportNavigations")]
    public virtual Airport DestinationAirportNavigation { get; set; } = null!;

    [InverseProperty("Route")]
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    [InverseProperty("Route")]
    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    [InverseProperty("Route")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
