using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Enums;

namespace vv_airline.Models.Data;

public partial class Aircraft
{
    [Key]
    [StringLength(255)]
    public string RegistrationNumber { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public List<Flight> Flights { get; set; } = new List<Flight>();

    public Model? Model { get; set; }

    public int RemainingSeats { get; set; }

    public AircraftEnums.Statuses Status { get; set; }
}
