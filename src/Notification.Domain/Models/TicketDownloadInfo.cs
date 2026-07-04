using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Models
{
    public class TicketDownloadInfo
    {
        public string? TicketCode { get; set; }
        public string? ZoneName { get; set; }
        public string? DownloadUrl { get; set; }
    }
}
