using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Models
{
    public record class TicketDownloadInfo
    {
        public string? TicketCode { get; init; }
        public string? ZoneName { get; init; }
    }
}
