using Rebus.Handlers;

using Microsoft.Extensions.Logging;
using Notification.Domain.Events;

namespace Notification.Infrastructure.Consumers
{
    public class ReservationConfirmedConsumer(ILogger<ReservationConfirmedConsumer> logger):IHandleMessages<ReservationConfirmedEvent>
    {
        public Task Handle(ReservationConfirmedEvent message)
        {
            if (message != null && logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Message received! Client: {Client} Event: {Event}", message.CustomerName, message.EventName);
            }

            return Task.CompletedTask;
        }
    }
}
