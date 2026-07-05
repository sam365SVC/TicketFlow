using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Configurations
{
    public class RabbitMqOptions
    {
        public const string SectionName = "RabbitMq";

        public string? Host { get; set; }
        public ushort Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
