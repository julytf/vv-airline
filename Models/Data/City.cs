using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class City
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("City")]
    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    [InverseProperty("City")]
    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    [InverseProperty("City")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
