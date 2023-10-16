using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Seat
{
    [Key]
    public long Id { get; set; }

    public byte Row { get; set; }

    public byte Col { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    public  SeatMap SeatMap { get; set; } = null!;

    public  SeatClass SeatClass { get; set; } = null!;

    public  SeatType? SeatType { get; set; } = null!;

    public  ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
