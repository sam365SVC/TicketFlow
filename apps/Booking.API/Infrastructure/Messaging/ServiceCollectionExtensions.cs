using Booking.API.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Config;
using Rebus.ServiceProvider;

namespace Booking.API.Infrastructure.Messaging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddRebus((configure, provider) =>
        {
            var options = provider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            var connectionString = $"amqp://{options.Username}:{options.Password}@{options.Host}:{options.Port}";

            // Booking.API todavía solo publica (no consume nada) - por eso es cliente "one way".
            // Falta configurar el Routing una vez que se definan los eventos a publicar.
            return configure
                .Transport(t => t.UseRabbitMqAsOneWayClient(connectionString));
        });

        services.AddScoped<IEventPublisher, RebusEventPublisher>();

        return services;
    }
}
