using IoTControlTower.Application.DTO;

namespace IoTControlTower.Application.Interface
{
    public interface IAuthenticateService
    {
        Task<bool> Authenticate(string email, string password);

        Task<string> GenerateToken(LoginDTO loginDTO);
    }
}
