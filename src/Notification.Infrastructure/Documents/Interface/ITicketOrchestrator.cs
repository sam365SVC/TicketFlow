using Notification.Domain.Models;
using TicketFlow.Shared.Events;

namespace Notification.Infrastructure.Documents.Interface
{
    public interface ITicketOrchestrator
    {
        Task<List<TicketDownloadInfo>> ProcessAndUploadTicketsAsync(ReservationConfirmedEvent reservationEvent, CancellationToken cancellationToken = default);
    }
}
