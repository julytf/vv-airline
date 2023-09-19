using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Table("Seat_Type")]
public partial class SeatType
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("surcharge")]
    public long? Surcharge { get; set; }

    [InverseProperty("SeatType")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
