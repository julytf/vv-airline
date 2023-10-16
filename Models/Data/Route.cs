using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Route
{
    [Key]
    public long Id { get; set; }

    public  Airport DepartureAirport { get; set; } = null!;

    public  Airport DestinationAirport { get; set; } = null!;

    public long? Distance { get; set; }

    public  ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public  ICollection<Price> Prices { get; set; } = new List<Price>();

    public  ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
