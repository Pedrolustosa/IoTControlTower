using IoTControlTower.Application.DTO;

namespace IoTControlTower.Application.Interface
{
    public interface IAuthenticateService
    {
        Task<bool> Authenticate(LoginDTO loginDTO);

        Task<string> GenerateToken(LoginDTO loginDTO);
    }
}
