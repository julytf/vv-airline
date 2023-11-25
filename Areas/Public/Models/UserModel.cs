
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace vv_airline.Areas.Public.Models;
public class UserModel
{
    // public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(50)]
    [DisplayName("Email")]
    public string? Email { get; set; } = null!;

    [Phone]
    [StringLength(20)]
    [DisplayName("Số điện thoại")]
    public string? PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [DisplayName("Mật khẩu")]
    public string? Password { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [DisplayName("Tên")]
    public string? FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [DisplayName("Họ")]
    public string? LastName { get; set; } = null!;

    [Required]
    [DisplayName("Ngày sinh")]
    public DateTime? DateOfBirth { get; set; }

    [DisplayName("Giới tính")]
    public bool? Gender { get; set; }

    [StringLength(12)]
    [DisplayName("Số CCCD")]
    public string? IdNumber { get; set; } = null!;

    [DisplayName("Thành phố/ Tỉnh")]
    public string? ProvinceCode { get; set; }

    [DisplayName("Quận/ Huyện")]
    public string? DistrictCode { get; set; }

    [DisplayName("Phường/ Thị trấn")]
    public string? WardCode { get; set; }

    [StringLength(255)]
    [DisplayName("Địa chỉ")]
    public string? Address { get; set; } = null!;

    [StringLength(255)]
    [DisplayName("Địa chỉ chi tiết ( Tòa nhà, số phòng, ...)")]
    public string? Address2 { get; set; } = null!;
}