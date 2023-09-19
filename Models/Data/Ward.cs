using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("DistrictId", Name = "IX_Wards_district_id")]
public partial class Ward
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("district_id")]
    public long DistrictId { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Ward")]
    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    [ForeignKey("DistrictId")]
    [InverseProperty("Wards")]
    public virtual District District { get; set; } = null!;

    [InverseProperty("Ward")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
