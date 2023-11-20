using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Schedule
{
    [Key]
    public long Id { get; set; }

    public bool HasTransit { get; set; } = false;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public long? Distance { get; set; }

    public List<Flight> Flights { get; set; } = new List<Flight>();

    public FlightRoute FlightRoute { get; set; } = null!;

    public List<Booking> Bookings { get; set; }

    [NotMapped]
    public IDictionary<String, long> Prices { get; set; } = new Dictionary<String, long>();

}
