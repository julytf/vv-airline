using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Areas.Admin.Models;

public partial class FlightRouteModel
{
    public long DepartureAirportId { get; set; }

    public long DestinationAirportId { get; set; }

    public long? Distance { get; set; }
}
