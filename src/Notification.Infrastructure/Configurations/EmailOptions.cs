using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Configurations
{
    public class EmailOptions
    {
        public const string SectionName = "EmailConfig";
        public required string Email { get; set; }
        public required string PasswordApp { get; set; }
    }
}
