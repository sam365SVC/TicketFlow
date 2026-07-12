using TicketFlow.Shared.Models;

namespace TicketFlow.Shared.Events
{
    public class ReservationConfirmedEvent
    {
        public string CustomerName { get; init; } = string.Empty;
        public string CustomerEmail { get; init; } = string.Empty;
        public string CustomerPhone { get; init; } = string.Empty;
        public string EventName { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public string Location { get; init; } = string.Empty;
        public string EventUrl { get; init; } = string.Empty;
        public List<TicketInfo> Tickets { get; init; } = [];
    }
}
