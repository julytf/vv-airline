using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class FlightRoute
{
    [Key]
    public long Id { get; set; }

    public  Airport DepartureAirport { get; set; } = null!;

    public  Airport DestinationAirport { get; set; } = null!;

    public long? Distance { get; set; }

    public List<Flight> Flights { get; set; } = new List<Flight>();

    public List<DefaultPrice> DefaultPrices { get; set; } = new List<DefaultPrice>();

    public List<Schedule> Schedules { get; set; } = new List<Schedule>();
}
