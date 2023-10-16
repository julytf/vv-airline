using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Passenger
{
    [Key]
    public long Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Type { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public bool? Gender { get; set; }

    public  Booking Booking { get; set; } = null!;

    public  ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
