using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Enums;

namespace vv_airline.Models.Data;

public partial class Ticket
{
    [Key]
    public long Id { get; set; }

    // public long Price { get; set; }

    public Flight Flight { get; set; } = null!;

    public Passenger Passenger { get; set; } = null!;

    public Seat Seat { get; set; } = null!;

    // public TicketEnums.Statuses Status { get; set; }
}
