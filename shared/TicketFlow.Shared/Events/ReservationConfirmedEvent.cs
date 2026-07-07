using TicketFlow.Shared.Models;

namespace TicketFlow.Shared.Events
{
    public class ReservationConfirmedEvent
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public List<TicketInfo> Tickets { get; set; } = [];
    }
}
