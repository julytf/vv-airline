using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Enums;

namespace vv_airline.Models.Data;

public partial class Passenger
{
    [Key]
    public long Id { get; set; }

    public PassengerEnums.Type Type { get; set; } = PassengerEnums.Type.Adult;

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

    [NotMapped]
    public string FullName
    {
        get
        {
            return LastName + " " + FirstName;
        }
    }
    public DateTime? DateOfBirth { get; set; }

    public bool? Gender { get; set; }

    public Booking Booking { get; set; } = null!;

    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
