using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("DistrictCode", Name = "idx_wards_district")]
public partial class Ward
{
    [Key]
    [Column("code")]
    [StringLength(20)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("name_en")]
    [StringLength(255)]
    public string? NameEn { get; set; }

    [Column("full_name")]
    [StringLength(255)]
    public string? FullName { get; set; }

    [Column("full_name_en")]
    [StringLength(255)]
    public string? FullNameEn { get; set; }

    [Column("code_name")]
    [StringLength(255)]
    public string? CodeName { get; set; }

    [Column("district_code")]
    [StringLength(20)]
    public string? DistrictCode { get; set; }

    [ForeignKey("DistrictCode")]
    [InverseProperty("Wards")]
    public virtual District? DistrictCodeNavigation { get; set; }

    [InverseProperty("Ward")]
    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    [InverseProperty("Ward")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
