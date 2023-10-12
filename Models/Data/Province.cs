using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Table("Provinces")]
public partial class Province
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
    public string FullName { get; set; } = null!;

    [Column("full_name_en")]
    [StringLength(255)]
    public string? FullNameEn { get; set; }

    [Column("code_name")]
    [StringLength(255)]
    public string? CodeName { get; set; }

    [InverseProperty("ProvinceCodeNavigation")]
    public virtual ICollection<District> Districts { get; set; } = new List<District>();
    
    [InverseProperty("Province")]
    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    [InverseProperty("Province")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
