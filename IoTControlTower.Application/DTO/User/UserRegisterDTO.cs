using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO.User
{
    public class UserRegisterDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
