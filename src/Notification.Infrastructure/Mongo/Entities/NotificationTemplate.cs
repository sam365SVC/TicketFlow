using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Mongo.Entities
{
    public class NotificationTemplate
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string SubjectTemplate { get; set; } = string.Empty;
        public string BodyTemplate { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
