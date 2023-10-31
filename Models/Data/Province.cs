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
    [StringLength(20)]
    public string Code { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? NameEn { get; set; }

    [StringLength(255)]
    public string FullName { get; set; } = null!;

    [StringLength(255)]
    public string? FullNameEn { get; set; }

    [StringLength(255)]
    public string? CodeName { get; set; }

    [InverseProperty("Province")]
    public List<District> Districts { get; set; } = new List<District>();

    public List<Airport> Airports { get; set; } = new List<Airport>();

    public List<User> Users { get; set; } = new List<User>();
}
