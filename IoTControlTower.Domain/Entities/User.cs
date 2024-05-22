using Microsoft.AspNetCore.Identity;

namespace IoTControlTower.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        public ICollection<Device> Devices { get; set; }
    }
}
