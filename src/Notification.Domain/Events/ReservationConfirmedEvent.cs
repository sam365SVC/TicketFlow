using Notification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Events
{
    public record class ReservationConfirmedEvent
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public List<TicketInfo> Tickets { get; set; } = [];
    }
}
