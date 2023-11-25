using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using vv_airline.Models.Data;

namespace vv_airline.Areas.SearchAndBooking.Models;
public class SearchModel
{
    [DisplayName("Sân bay đi")]
    [Required]
    public long DepartureAirportId { get; set; }

    [DisplayName("Sân bay đến")]
    [Required]
    public long DestinationAirportId { get; set; }

    // [DisplayName("Hạng ghế")]
    // public long SeatClass { get; set; }

    public bool IsRoundtrip { get; set; }

    [DisplayName("Số người lớn")]
    [Required]
    public byte Adults { get; set; }

    [DisplayName("Số trẻ em (< 12 tuổi)")]
    [Required]
    public byte Children { get; set; }

    [DisplayName("Ngày đi")]
    [Required]
    public DateTime DepartureDate { get; set; }

    [DisplayName("Ngày Về")]
    public DateTime? ReturnDate { get; set; }

    public void Validate(ModelStateDictionary ModelState)
    {
        if (IsRoundtrip && ReturnDate == null)
        {
            ModelState.AddModelError("ReturnDate", "error");
            return;
        }
    }
}