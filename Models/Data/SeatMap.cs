using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class SeatMap
{
    [Key]
    public long ModelId { get; set; }

    public byte NoRow { get; set; }

    public byte NoCol { get; set; }

    public  List<AisleCol> AisleCols { get; set; } = new List<AisleCol>();

    public  List<ExitRow> ExitRows { get; set; } = new List<ExitRow>();

    public  Model Model { get; set; } = null!;

    public  List<Seat> Seats { get; set; } = new List<Seat>();
}
