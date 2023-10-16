using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Booking
{
    [Key]
    public long Id { get; set; }


    public byte Adults { get; set; }

    public byte? Children { get; set; }

    public bool? IsRoundtrip { get; set; }

    public decimal TotalPrice { get; set; }

    public  ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    public  ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public  User? User { get; set; }
    
    public  ICollection<Service> Services { get; set; } = new List<Service>();
}
