using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Areas.Admin.Models;

public partial class ModelModel
{

    public string Name { get; set; } = null!;

}
