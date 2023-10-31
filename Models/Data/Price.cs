using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class Price
{
    [Key]
    public long Id { get; set; }

    public decimal Value { get; set; }

    public  FlightRoute FlightRoute { get; set; } = null!;

    public  SeatClass SeatClass { get; set; } = null!;
}
