using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Table("Seat")]
[Microsoft.EntityFrameworkCore.Index("ModelId", Name = "IX_Seat_model_id")]
[Microsoft.EntityFrameworkCore.Index("SeatClassId", Name = "IX_Seat_seat_class_id")]
[Microsoft.EntityFrameworkCore.Index("SeatTypeId", Name = "IX_Seat_seat_type_id")]
public partial class Seat
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("model_id")]
    public long ModelId { get; set; }

    [Column("row")]
    public byte Row { get; set; }

    [Column("col")]
    public byte Col { get; set; }

    [Column("seat_class_id")]
    public long SeatClassId { get; set; }

    [Column("seat_type_id")]
    public long SeatTypeId { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("Seats")]
    public virtual SeatMap Model { get; set; } = null!;

    [ForeignKey("SeatClassId")]
    [InverseProperty("Seats")]
    public virtual SeatClass SeatClass { get; set; } = null!;

    [ForeignKey("SeatTypeId")]
    [InverseProperty("Seats")]
    public virtual SeatType SeatType { get; set; } = null!;

    [InverseProperty("Seat")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
