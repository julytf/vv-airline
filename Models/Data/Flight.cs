using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Flight
{
    public Flight() { }

    [Key]
    public long Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public  Aircraft Aircraft { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public  Route Route { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
