using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Enums;

namespace vv_airline.Areas.Admin.Models;

public partial class AircraftModel
{
    [StringLength(255)]
    public string RegistrationNumber { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public long ModelId { get; set; }

    public AircraftEnums.Statuses Status { get; set; }
}
