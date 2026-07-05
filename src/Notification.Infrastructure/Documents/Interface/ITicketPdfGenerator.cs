using Notification.Domain.Events;
using Notification.Domain.Models;

namespace Notification.Infrastructure.Documents.Interface
{
    public interface ITicketPdfGenerator
    {
        byte[] GeneratePdf(ReservationConfirmedEvent eventInfo, TicketInfo ticket, byte[] qrImageBytes);
    }
}
