using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Mongo.Entities
{
    public class NotificationDocument
    {
        public string Id { get; set; } = null!;
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string NotificationType { get; set; }=string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; }= string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsPaymentAutorized { get; set; }
        public string ExternalTransactionId { get; set; } = string.Empty;
        public List<TicketItemInfo> GeneratedTickets { get; set; } = [];
        public string TicketFileUrl { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public int Attempts { get; set; } = 0;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public string TemplateCode { get; set; } = null!;//logical reference NotificationTemplate
    }
}
