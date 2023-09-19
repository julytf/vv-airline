using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("ModelId", Name = "IX_Aircrafts_model_id")]
public partial class Aircraft
{
    [Key]
    [Column("registration_number")]
    [StringLength(255)]
    [Unicode(false)]
    public string RegistrationNumber { get; set; } = null!;

    [Column("model_id")]
    public long? ModelId { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("AircraftRegistrationNumberNavigation")]
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    [ForeignKey("ModelId")]
    [InverseProperty("Aircraft")]
    public virtual Model? Model { get; set; }
}
