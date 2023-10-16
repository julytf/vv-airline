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

    public bool? HasTransit { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public long? Distance { get; set; }

    public  ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public  Route Route { get; set; } = null!;

    public  ICollection<Booking> Bookings { get; set; }
}
