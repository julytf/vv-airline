﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class DefaultPrice
{
    [Key]
    public long Id { get; set; }

    public long Value { get; set; }

    public  FlightRoute FlightRoute { get; set; } = null!;

    public  SeatClass SeatClass { get; set; } = null!;
}
