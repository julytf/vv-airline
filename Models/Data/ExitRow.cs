using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class ExitRow
{
    [Key]
    public long Id { get; set; }

    public byte Value { get; set; }

    public SeatMap SeatMap { get; set; } = null!;
}
