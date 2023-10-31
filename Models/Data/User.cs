using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class User : IdentityUser
{
    // [Key]
    // public string Id { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
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

    [StringLength(12)]
    [Unicode(false)]
    public string? IdNumber { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Address2 { get; set; }

    public List<Booking> Bookings { get; set; } = new List<Booking>();

    public Province? Province { get; set; }

    public District? District { get; set; }

    public Ward? Ward { get; set; }

}
