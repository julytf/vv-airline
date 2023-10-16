using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class District
{
    [Key]
    [StringLength(20)]
    public string Code { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? NameEn { get; set; }

    [StringLength(255)]
    public string? FullName { get; set; }

    [StringLength(255)]
    public string? FullNameEn { get; set; }

    [StringLength(255)]
    public string? CodeName { get; set; }

    [StringLength(20)]
    public  Province? Province { get; set; }

    public  ICollection<Ward> Wards { get; set; } = new List<Ward>();

    public  ICollection<Airport> Airports { get; set; } = new List<Airport>();

    public  ICollection<User> Users { get; set; } = new List<User>();
}
