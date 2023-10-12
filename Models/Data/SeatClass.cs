using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class SeatClass
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("SeatClass")]
    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    [InverseProperty("SeatClass")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
