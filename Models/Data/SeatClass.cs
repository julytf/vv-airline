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

    public List<Price> Prices { get; set; } = new List<Price>();
    public List<DefaultPrice> DefaultPrices { get; set; } = new List<DefaultPrice>();

    public List<Seat> Seats { get; set; } = new List<Seat>();
}
