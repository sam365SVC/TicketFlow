using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Models;
using Notification.Infrastructure.Configurations;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Notification.Infrastructure.Messaging.Interface
{
    public class RabbitMqPublisherService(
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqPublisherService> logger
        ) : IMessageBusService
    {
        private const string QueueName = "whatsapp_notification_queue";

        public async Task PublishWhatsappNotification(ReservationWhatsappModel notification)
        {
            var connectionString = $"amqp://{options.Value.Username}:{options.Value.Password}@{options.Value.Host}:{options.Value.Port}";

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            var json = JsonSerializer.Serialize(notification);
            var body=Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: QueueName,
                body: body
                );
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ Whatsapp event public in RabbitMQ from {CustomerPhone}",notification.CustomerPhone);
            }
        }
    }
}
