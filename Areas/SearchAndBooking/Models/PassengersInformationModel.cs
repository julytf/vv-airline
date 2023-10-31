using System.ComponentModel.DataAnnotations;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Areas.SearchAndBooking.Models;
public class PassengersInformationModel
{
    public class PassengerInformationModel
    {
        public PassengerEnums.Type Type { get; set; } = PassengerEnums.Type.Adult;

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        public string LastName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public bool? Gender { get; set; }
    }
    public PassengerInformationModel[]? Adults { get; set; } = Array.Empty<PassengerInformationModel>();
    public PassengerInformationModel[]? Children { get; set; } = Array.Empty<PassengerInformationModel>();
}