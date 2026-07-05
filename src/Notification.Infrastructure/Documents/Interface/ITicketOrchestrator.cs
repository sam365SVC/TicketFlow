using Notification.Domain.Events;
using Notification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Documents.Interface
{
    public interface ITicketOrchestrator
    {
        Task<List<TicketDownloadInfo>> ProcessAndUploadTicketsAsync(ReservationConfirmedEvent reservationEvent, CancellationToken cancellationToken = default);
    }
}
