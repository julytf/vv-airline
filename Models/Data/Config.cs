using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Config
{
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string Key { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Value { get; set; } = null!;
}
