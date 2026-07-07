using TicketFlow.Shared.Models;
using TicketFlow.Shared.Events;

namespace Notification.Infrastructure.Documents.Interface
{
    public interface ITicketPdfGenerator
    {
        byte[] GeneratePdf(ReservationConfirmedEvent eventInfo, TicketInfo ticket, byte[] qrImageBytes);
    }
}
