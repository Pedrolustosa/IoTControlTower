using Microsoft.AspNetCore.Identity;

namespace IoTControlTower.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}
