using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("ModelId", Name = "aisle_col_index_1")]
public partial class AisleCol
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("model_id")]
    public long ModelId { get; set; }

    [Column("value")]
    public byte Value { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("AisleCols")]
    public virtual SeatMap Model { get; set; } = null!;
}
