using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Airport
{
    [Key]
    public long Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public Province? Province { get; set; }

    public District? District { get; set; }

    public Ward? Ward { get; set; }

    [InverseProperty("DepartureAirport")]
    public ICollection<Route> RouteDepartures { get; set; } = new List<Route>();

    [InverseProperty("DestinationAirport")]
    public ICollection<Route> RouteDestinations { get; set; } = new List<Route>();

}
