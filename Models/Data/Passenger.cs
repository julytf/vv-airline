using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("BookingId", Name = "IX_Passengers_booking_id")]
public partial class Passenger
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("booking_id")]
    public long BookingId { get; set; }

    [Column("index")]
    public int Index { get; set; }

    [Column("type")]
    [StringLength(20)]
    [Unicode(false)]
    public string Type { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("date_of_birth", TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [Column("gender")]
    public bool? Gender { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("Passengers")]
    public virtual Booking Booking { get; set; } = null!;

    [InverseProperty("Passenger")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
