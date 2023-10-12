using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class SeatMap
{
    [Key]
    [Column("model_id")]
    // [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ModelId { get; set; }

    [Column("no_row")]
    public byte NoRow { get; set; }

    [Column("no_col")]
    public byte NoCol { get; set; }

    [InverseProperty("Model")]
    public virtual ICollection<AisleCol> AisleCols { get; set; } = new List<AisleCol>();

    [InverseProperty("Model")]
    public virtual ICollection<ExitRow> ExitRows { get; set; } = new List<ExitRow>();

    [ForeignKey("ModelId")]
    [InverseProperty("SeatMap")]
    public virtual Model Model { get; set; } = null!;

    [InverseProperty("Model")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
