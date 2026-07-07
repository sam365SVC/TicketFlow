using TicketFlow.Shared.Events;
using Notification.Domain.Models;
using Notification.Infrastructure.Documents.Interface;
using Notification.Infrastructure.Storage;

namespace Notification.Infrastructure.Documents
{
    public class TicketOrchestrator(
        IQrGenerator qrGenerator,
        ITicketPdfGenerator ticketPdfGenerator,
        IAwsS3Client s3Client
    ):ITicketOrchestrator
    {
        public async Task<List<TicketDownloadInfo>> ProcessAndUploadTicketsAsync(ReservationConfirmedEvent reservationEvent, CancellationToken cancellationToken = default)
        {
            var uploadedTicketUrls = new List<TicketDownloadInfo>();

            var expireDate = reservationEvent.EventDate.AddHours(4);

            foreach (var ticket in reservationEvent.Tickets)
            {
                if (string.IsNullOrWhiteSpace(ticket.TicketCode)) continue;
                
                var qrBytes=qrGenerator.GenerateValidationQrCode(ticket.TicketCode);

                var pdfBytes = ticketPdfGenerator.GeneratePdf(reservationEvent, ticket, qrBytes);

                string codeKey = $"tickets/{reservationEvent.EventName}-{ticket.TicketCode}.pdf";

                var ticketUrl = await s3Client.UploadTicketAsync(codeKey,expireDate, pdfBytes, cancellationToken);

                var dowloadTicket = new TicketDownloadInfo
                {
                    TicketCode = ticket.TicketCode,
                    ZoneName = ticket.ZoneName,
                    Url = ticketUrl,
                }; 

                uploadedTicketUrls.Add(dowloadTicket);
            }

            return uploadedTicketUrls;
        }
    }
}
