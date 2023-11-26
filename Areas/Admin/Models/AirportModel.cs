using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Models;

public partial class AirportModel
{
    [StringLength(255)]
    public string Name { get; set; } = null!;
    [StringLength(255)]
    public string? NameEn { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public string? ProvinceCode { get; set; }

    public string? DistrictCode { get; set; }

    public string? WardCode { get; set; }

}
