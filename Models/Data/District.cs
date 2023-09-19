using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("CityId", Name = "IX_Districts_city_id")]
public partial class District
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("city_id")]
    public long CityId { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("District")]
    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    [ForeignKey("CityId")]
    [InverseProperty("Districts")]
    public virtual City City { get; set; } = null!;

    [InverseProperty("District")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [InverseProperty("District")]
    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
