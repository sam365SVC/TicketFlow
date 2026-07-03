using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Mongo.Entities
{
    public class TicketItemInfo
    {
        public int BookingItemId { get; set; }
        public string TicketCode { get; set; } = string.Empty;
    }
}
