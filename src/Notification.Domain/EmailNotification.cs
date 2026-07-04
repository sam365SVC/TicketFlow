using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain
{
    public class EmailNotification(string recipient, string subject, string body)
    {
        public string? Recipient { get; set; } = recipient;
        public string? Subject { get; set; } = subject;
        public string? Body { get; set; } = body;

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    }
}
