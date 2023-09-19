using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Service
{
    [Key]
    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("price", TypeName = "decimal(18, 0)")]
    public decimal? Price { get; set; }

    [ForeignKey("ServiceName")]
    [InverseProperty("ServiceNames")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
