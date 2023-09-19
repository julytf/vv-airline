using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Table("Exit_rows")]
[Index("ModelId", Name = "exit_row_index_0")]
public partial class ExitRow
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("model_id")]
    public long ModelId { get; set; }

    [Column("value")]
    public byte Value { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("ExitRows")]
    public virtual SeatMap Model { get; set; } = null!;
}
