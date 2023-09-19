
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace vv_airline.Areas.Public.Models;
public class RegisterModel
{
    // public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string? Email { get; set; } = null!;

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string? Password { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string? LastName { get; set; } = null!;

    [Required]
    public DateTime? DateOfBirth { get; set; }

    public bool? Gender { get; set; }

    [StringLength(12)]
    public string? IdNumber { get; set; } = null!;

    public long? CityId { get; set; }

    public long? DistrictId { get; set; }

    public long? WardId { get; set; }

    [StringLength(255)]
    public string? Address { get; set; } = null!;

    [StringLength(255)]
    public string? Address2 { get; set; } = null!;
}