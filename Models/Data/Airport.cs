using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("CityId", Name = "IX_Airports_city_id")]
[Index("DistrictId", Name = "IX_Airports_district_id")]
[Index("WardId", Name = "IX_Airports_ward_id")]
public partial class Airport
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("longtitude")]
    public double? Longtitude { get; set; }

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("city_id")]
    public long? CityId { get; set; }

    [Column("district_id")]
    public long? DistrictId { get; set; }

    [Column("ward_id")]
    public long? WardId { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Airports")]
    public virtual City? City { get; set; }

    [ForeignKey("DistrictId")]
    [InverseProperty("Airports")]
    public virtual District? District { get; set; }

    [InverseProperty("DepartureAirportNavigation")]
    public virtual ICollection<Route> RouteDepartureAirportNavigations { get; set; } = new List<Route>();

    [InverseProperty("DestinationAirportNavigation")]
    public virtual ICollection<Route> RouteDestinationAirportNavigations { get; set; } = new List<Route>();

    [ForeignKey("WardId")]
    [InverseProperty("Airports")]
    public virtual Ward? Ward { get; set; }
}
