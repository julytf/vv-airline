using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Models;

public partial class ScheduleModel
{
    // public bool HasTransit { get; set; } = false;

    // public DateTime DepartureTime { get; set; }

    // public DateTime ArrivalTime { get; set; }

    public long?[] FlightIds { get; set; } = new long?[2];

    public long EconomyPrice { get; set; }
    public long BusinessPrice { get; set; }

    // public FlightRoute FlightRoute { get; set; } = null!;

    // public List<Price> Prices { get; set; } = new List<Price>();
}
