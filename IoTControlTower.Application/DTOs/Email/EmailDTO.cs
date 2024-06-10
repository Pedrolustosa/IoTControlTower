namespace IoTControlTower.Application.DTO.Email;

public record EmailDTO(string From, string SmtpServer, int Port, string UserName, string Password);
