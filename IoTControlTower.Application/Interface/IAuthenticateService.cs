namespace IoTControlTower.Application.Interface
{
    public interface IAuthenticateService
    {
        Task<bool> Authenticate(string email, string password);

        public string GenerateToken(string email);
    }
}
