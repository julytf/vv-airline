using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

[Microsoft.EntityFrameworkCore.Index("NormalizedEmail", Name = "EmailIndex")]
[Microsoft.EntityFrameworkCore.Index("ProvinceCode", Name = "IX_Users_province_code")]
[Microsoft.EntityFrameworkCore.Index("DistrictCode", Name = "IX_Users_district_code")]
[Microsoft.EntityFrameworkCore.Index("WardCode", Name = "IX_Users_ward_code")]
public partial class User : IdentityUser
{
    // [Key]
    // public string Id { get; set; } = null!;
    
    [Column("first_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("date_of_birth", TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [Column("gender")]
    public bool? Gender { get; set; }

    [Column("id_number")]
    [StringLength(12)]
    [Unicode(false)]
    public string? IdNumber { get; set; }

    [Column("province_id")]
    public string? ProvinceCode { get; set; }

    [Column("district_id")]
    public string? DistrictCode { get; set; }

    [Column("ward_id")]
    public string? WardCode { get; set; }

    [Column("address")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Address { get; set; }

    [Column("address2")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Address2 { get; set; }

    // [StringLength(256)]
    // public string? UserName { get; set; }

    // [StringLength(256)]
    // public string? NormalizedUserName { get; set; }

    // [StringLength(256)]
    // public string? Email { get; set; }

    // [StringLength(256)]
    // public string? NormalizedEmail { get; set; }

    // public bool EmailConfirmed { get; set; }

    // public string? PasswordHash { get; set; }

    // public string? SecurityStamp { get; set; }

    // public string? ConcurrencyStamp { get; set; }

    // public string? PhoneNumber { get; set; }

    // public bool PhoneNumberConfirmed { get; set; }

    // public bool TwoFactorEnabled { get; set; }

    // public DateTimeOffset? LockoutEnd { get; set; }

    // public bool LockoutEnabled { get; set; }

    // public int AccessFailedCount { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("ProvinceCode")]
    [InverseProperty("Users")]
    public virtual Province? Province { get; set; }

    [ForeignKey("DistrictCode")]
    [InverseProperty("Users")]
    public virtual District? District { get; set; }

    // [InverseProperty("User")]
    // public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();

    // [InverseProperty("User")]
    // public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    // [InverseProperty("User")]
    // public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

    [ForeignKey("WardCode")]
    [InverseProperty("Users")]
    public virtual Ward? Ward { get; set; }

    // [ForeignKey("UserId")]
    // [InverseProperty("Users")]
    // public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
