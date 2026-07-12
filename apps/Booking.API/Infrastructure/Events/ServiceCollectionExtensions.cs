using Booking.API.Application.Abstractions;
using Booking.API.Application.Events;
using Booking.API.Infrastructure.Repositories;

namespace Booking.API.Infrastructure.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingEventServices(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<CreateEventService>();
        services.AddScoped<GetEventByIdService>();
        services.AddScoped<GetPublishedEventsService>();

        return services;
    }
}
