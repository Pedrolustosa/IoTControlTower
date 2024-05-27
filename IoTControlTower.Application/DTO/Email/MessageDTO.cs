using MimeKit;

namespace IoTControlTower.Application.DTO.Email
{
    public class MessageDTO(IEnumerable<string> mailboxAddresses, string subject, string content)
    {
        public List<MailboxAddress> MailboxAddresses { get; set; } = [.. mailboxAddresses.Select(ma => new MailboxAddress("email", ma))];
        public string Subject { get; set; } = subject;
        public string Content { get; set; } = content;
    }
}
