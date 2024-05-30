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
            _logger.LogInformation("SendEmail() - Preparing to send email to: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));

            try
            {
                var emailMessage = CreateEmailMessage(message);
                Send(emailMessage);
                _logger.LogInformation("SendEmail() - Email sent successfully to: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendEmail() - Error occurred while sending email to: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
                throw;
            }
        }

        private MimeMessage CreateEmailMessage(MessageDTO message)
        {
            try
            {
                _logger.LogInformation("CreateEmailMessage() - Creating email message for: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Welcome to IoT Control Tower", _emailDTO.From));
                emailMessage.To.AddRange(message.MailboxAddresses);
                emailMessage.Subject = message.Subject;

                string htmlContent = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            color: #333;
                            line-height: 1.6;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background: #fff;
                            padding: 20px;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #2c3e50;
                            text-align: center;
                        }}
                        p {{
                            margin: 20px 0;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin: 20px 0;
                            font-size: 16px;
                            color: #fff;
                            background-color: #3498db;
                            border-radius: 5px;
                            text-align: center;
                            text-decoration: none;
                            transition: background-color 0.3s;
                        }}
                        .button:hover {{
                            background-color: #2980b9;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Welcome to IoT Control Tower!</h1>
                        <p>{message.Content}</p>
                        <a href='#' class='button'>Confirm Your Email</a>
                    </div>
                </body>
                </html>";

                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlContent };

                _logger.LogInformation("CreateEmailMessage() - Email message created successfully for: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
                return emailMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEmailMessage() - Error occurred while creating email message for: {Recipients}", string.Join(", ", message.MailboxAddresses.Select(a => a.Address)));
                throw;
            }
        }

        private void Send(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                _logger.LogInformation("Send() - Connecting to SMTP server: {SmtpServer}", _emailDTO.SmtpServer);

                client.Connect(_emailDTO.SmtpServer, _emailDTO.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailDTO.UserName, _emailDTO.Password);

                _logger.LogInformation("Send() - Sending email to: {Recipients}", string.Join(", ", message.To.Select(a => ((MailboxAddress)a).Address)));
                client.Send(message);

                _logger.LogInformation("Send() - Email sent successfully to: {Recipients}", string.Join(", ", message.To.Select(a => ((MailboxAddress)a).Address)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Send() - Error occurred while sending email to: {Recipients}", string.Join(", ", message.To.Select(a => ((MailboxAddress)a).Address)));
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
                _logger.LogInformation("Send() - Disconnected from SMTP server: {SmtpServer}", _emailDTO.SmtpServer);
            }
        }
    }
}
