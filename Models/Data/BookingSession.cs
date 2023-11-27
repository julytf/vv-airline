using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class BookingSession
{
    [Key]
    public long Id { get; set; }

    public string Data { get; set; } = null!;


}
