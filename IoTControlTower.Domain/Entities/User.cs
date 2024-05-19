using Microsoft.AspNetCore.Identity;

namespace IoTControlTower.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
