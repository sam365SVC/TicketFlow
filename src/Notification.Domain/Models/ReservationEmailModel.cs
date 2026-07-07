using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Models
{
    public class ReservationEmailModel
    {
        public string? CustomerName { get; set; }
        public string? EventName { get; set; }
        public string? EventDate { get; set; }
        public List<TicketDownloadInfo> Tickets { get; set; } = [];
    }
}
