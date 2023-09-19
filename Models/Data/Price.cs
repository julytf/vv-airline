using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Index("RouteId", Name = "IX_Prices_route_id")]
[Index("SeatClassId", Name = "IX_Prices_seat_class_id")]
public partial class Price
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("route_id")]
    public long RouteId { get; set; }

    [Column("seat_class_id")]
    public long SeatClassId { get; set; }

    [Column("value", TypeName = "decimal(18, 0)")]
    public decimal Value { get; set; }

    [ForeignKey("RouteId")]
    [InverseProperty("Prices")]
    public virtual Route Route { get; set; } = null!;

    [ForeignKey("SeatClassId")]
    [InverseProperty("Prices")]
    public virtual SeatClass SeatClass { get; set; } = null!;
}
