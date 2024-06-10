using IoTControlTower.Application.DTO.Email;

namespace IoTControlTower.Application.Interface
{
    public interface IEmailService
    {
        void SendEmail(MessageDTO message);
    }
}
