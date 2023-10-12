using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("ProvinceCode", Name = "IX_Airports_province_code")]
[Microsoft.EntityFrameworkCore.Index("DistrictCode", Name = "IX_Airports_district_code")]
[Microsoft.EntityFrameworkCore.Index("WardCode", Name = "IX_Airports_ward_code")]
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

    [Column("province_code")]
    public string? ProvinceCode { get; set; }

    [Column("district_code")]
    public string? DistrictCode { get; set; }

    [Column("ward_code")]
    public string? WardCode { get; set; }

    [ForeignKey("ProvinceCode")]
    [InverseProperty("Airports")]
    public virtual Province? Province { get; set; }

    [ForeignKey("DistrictCode")]
    [InverseProperty("Airports")]
    public virtual District? District { get; set; }

    [InverseProperty("DepartureAirportNavigation")]
    public virtual ICollection<Route> RouteDepartureAirportNavigations { get; set; } = new List<Route>();

    [InverseProperty("DestinationAirportNavigation")]
    public virtual ICollection<Route> RouteDestinationAirportNavigations { get; set; } = new List<Route>();

    [ForeignKey("WardCode")]
    [InverseProperty("Airports")]
    public virtual Ward? Ward { get; set; }
}
