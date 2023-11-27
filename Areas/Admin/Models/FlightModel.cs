using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Areas.Admin.Models;

public partial class FlightModel
{
    public string AircraftRegistrationNumber { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public int RemainingSeats { get; set; }

    public long FlightRouteId { get; set; }

    public string? Status { get; set; }

}
