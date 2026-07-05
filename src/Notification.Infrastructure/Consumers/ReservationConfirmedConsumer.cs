using Microsoft.Extensions.Logging;
using Notification.Domain.Events;
using Notification.Domain.Models;
using Notification.Infrastructure.Documents;
using Notification.Infrastructure.Documents.Interface;
using Notification.Infrastructure.Email.Interface;
using Rebus.Handlers;

namespace Notification.Infrastructure.Consumers
{
    public class ReservationConfirmedConsumer(
        ILogger<ReservationConfirmedConsumer> logger,
        ITicketOrchestrator ticketOrchestrator,
        IEmailService emailService
    ):IHandleMessages<ReservationConfirmedEvent>
    {
        public async Task Handle(ReservationConfirmedEvent message)
        {
            // 1. El orquestador hace todo el trabajo sucio y nos devuelve las URLs de S3
            List<TicketDownloadInfo> ticketUrls = await ticketOrchestrator.ProcessAndUploadTicketsAsync(message);

            var emailModel = new ReservationEmailModel
            {
                CustomerName = message.CustomerName,
                EventName = message.EventName,
                EventData = message.EventDate,
                Tickets = ticketUrls
            };

            await emailService.SendTicketEmailAsync(message.CustomerEmail, emailModel);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ ¡Proceso completado! Tickets generados y correo enviado a {Email}.", message.CustomerEmail);
            }
        }
    }
}
