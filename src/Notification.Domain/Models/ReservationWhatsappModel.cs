using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Models
{
    public class ReservationWhatsappModel
    {
        public string? CustomerName { get; init; }
        public required string CustomerPhone { get; init; }
        public string? EventName { get; init; }
        public List<TicketDownloadInfo> Tickets { get; init; } = [];
    }
}
