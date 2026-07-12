using Microsoft.Extensions.Logging;
using Notification.Domain.Models;
using Notification.Infrastructure.Documents.Interface;
using Notification.Infrastructure.Email.Interface;
using Notification.Infrastructure.Messaging.Interface;
using Rebus.Handlers;
using TicketFlow.Shared.Events;

namespace Notification.Infrastructure.Consumers
{
    public class ReservationConfirmedConsumer(
        ILogger<ReservationConfirmedConsumer> logger,
        ITicketOrchestrator ticketOrchestrator,
        IEmailService emailService,
        IMessageBusService whatsappBus
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
                EventDate = message.EventDate.ToString("dd/MM/yyyy HH:mm"),
                Tickets = ticketUrls
            };

            await emailService.SendTicketEmailAsync(message.CustomerEmail, emailModel);

            if (!string.IsNullOrWhiteSpace(message.CustomerPhone))
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Phone number detec. Ready for send Whatsapp from {Phone}", message.CustomerPhone);
                }
                var whatsappModel = new ReservationWhatsappModel
                {
                    CustomerName = message.CustomerName,
                    CustomerPhone = message.CustomerPhone,
                    EventName = message.EventName,
                    Tickets = ticketUrls
                };
                await whatsappBus.PublishWhatsappNotification(whatsappModel);
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("✅ Message publuc success in queue Whatsapp.Worker");
                }
            }
            if (string.IsNullOrWhiteSpace(message.CustomerEmail)&& string.IsNullOrWhiteSpace(message.CustomerPhone))
            {
                logger.LogError("⚠️ User don't send email or phone client ");
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ ¡Proceso completado! Tickets generados y correo enviado a {Email}.", message.CustomerEmail);
            }
        }
    }
}
