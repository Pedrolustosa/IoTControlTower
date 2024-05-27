using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using IoTControlTower.Application.DTO.Email;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.Application.Service
{
    public class EmailService(EmailDTO emailDTO, ILogger<EmailService> logger) : IEmailService
    {
        private readonly EmailDTO _emailDTO = emailDTO;
        private readonly ILogger<EmailService> _logger = logger;

        public void SendEmail(MessageDTO message)
        {
            _logger.LogInformation("SendEmail - Preparing to send email to: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));

            try
            {
                var emailMessage = CreateEmailMessage(message);
                Send(emailMessage);
                _logger.LogInformation("SendEmail - Email sent successfully to: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendEmail - Error occurred while sending email to: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
                throw;
            }
        }

        private MimeMessage CreateEmailMessage(MessageDTO message)
        {
            try
            {
                _logger.LogInformation("CreateEmailMessage - Creating email message for: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("email", _emailDTO.From));
                emailMessage.To.AddRange(message.MailboxAddresses);
                emailMessage.Subject = message.Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

                _logger.LogInformation("CreateEmailMessage - Email message created successfully for: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
                return emailMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEmailMessage - Error occurred while creating email message for: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
                throw;
            }
        }

        private void Send(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                _logger.LogInformation("Send - Connecting to SMTP server: {SmtpServer}", _emailDTO.SmtpServer);

                client.Connect(_emailDTO.SmtpServer, _emailDTO.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailDTO.UserName, _emailDTO.Password);

                _logger.LogInformation("Send - Sending email to: {Recipients}", string.Join(", ", message.To.Select(a => ((MailboxAddress)a).Address)));
                client.Send(message);

                _logger.LogInformation("Send - Email sent successfully to: {Recipients}", string.Join(", ", message.To.Select(a => ((MailboxAddress)a).Address)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Send - Error occurred while sending email to: {Recipients}", string.Join(", ", message.To.Select(a => ((MailboxAddress)a).Address)));
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
                _logger.LogInformation("Send - Disconnected from SMTP server: {SmtpServer}", _emailDTO.SmtpServer);
            }
        }
    }
}
