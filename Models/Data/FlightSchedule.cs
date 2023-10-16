// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;

// namespace vv_airline.Models.Data;

// [PrimaryKey("FlightId", "ScheduleId")]
// public partial class FlightSchedule
// {
//     [Key]
//     public long FlightId { get; set; }

//     [ForeignKey("FlightId")]
//     public  Schedule Schedule { get; set; }
    
//     [Key]
//     public long ScheduleId { get; set; }

//     [ForeignKey("ScheduleId")]
//     public  Flight Flight { get; set; }
// }
