using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class SeatClass
{
    [Key]
    public long Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public  ICollection<Price> Prices { get; set; } = new List<Price>();

    public  ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
