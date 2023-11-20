using System.ComponentModel.DataAnnotations;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;
namespace vv_airline.Areas.SearchAndBooking.Models.searchAndBookingService;
public class PassengerModel
{
    public PassengerEnums.Type Type { get; set; } = PassengerEnums.Type.Adult;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName
    {
        get
        {
            return LastName + " " + FirstName;
        }
    }
    public DateTime? DateOfBirth { get; set; }
    public bool? Gender { get; set; }
}