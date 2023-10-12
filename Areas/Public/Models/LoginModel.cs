using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vv_airline.Areas.Public.Models;
public class LoginModel
{
    [Required]
    [EmailAddress]
    [StringLength(50)]
    [DisplayName("Email")]
    public string? Email { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [DisplayName("Mật khẩu")]
    public string? Password { get; set; } = null!;
}