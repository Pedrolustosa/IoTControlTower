using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
