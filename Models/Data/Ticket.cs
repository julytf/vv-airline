using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("FlightId", Name = "IX_Tickets_flight_id")]
[Index("PassengerId", Name = "IX_Tickets_passenger_id")]
[Index("SeatId", Name = "IX_Tickets_seat_id")]
public partial class Ticket
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("flight_id")]
    public long FlightId { get; set; }

    [Column("seat_id")]
    public long SeatId { get; set; }

    [Column("passenger_id")]
    public long PassengerId { get; set; }

    [Column("price", TypeName = "decimal(18, 0)")]
    public decimal Price { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("Tickets")]
    public virtual Flight Flight { get; set; } = null!;

    [ForeignKey("PassengerId")]
    [InverseProperty("Tickets")]
    public virtual Passenger Passenger { get; set; } = null!;

    [ForeignKey("SeatId")]
    [InverseProperty("Tickets")]
    public virtual Seat Seat { get; set; } = null!;
}
