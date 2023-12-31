﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class AisleCol
{
    [Key]
    public long Id { get; set; }

    public char Value { get; set; }

    public  SeatMap SeatMap { get; set; } = null!;
}
