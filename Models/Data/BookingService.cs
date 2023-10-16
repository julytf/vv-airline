// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;

// namespace vv_airline.Models.Data;

// [PrimaryKey("BookingId", "ServiceId")]
// public partial class BookingService
// {
//     [Key]
//     public long BookingId { get; set; }

//     [Key]
//     public long ServiceId { get; set; }

//     public  Booking Booking { get; set; } = null!;

//     public  Service Service { get; set; } = null!;
// }
