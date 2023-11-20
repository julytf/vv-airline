using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Enums;

namespace vv_airline.Models.Data;

public partial class Seat
{
    [Key]
    public long Id { get; set; }

    public byte Row { get; set; }

    public char Col { get; set; }

    public SeatEnums.Statuses Status { get; set; }

    public SeatMap SeatMap { get; set; } = null!;

    public SeatClass SeatClass { get; set; } = null!;

    public SeatType? SeatType { get; set; } = null!;

    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
