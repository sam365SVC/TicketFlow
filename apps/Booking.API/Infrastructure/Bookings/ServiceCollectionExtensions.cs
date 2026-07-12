using Booking.API.Application.Abstractions;
using Booking.API.Application.Bookings;
using Booking.API.Application.Payments;
using Booking.API.Infrastructure.Repositories;

namespace Booking.API.Infrastructure.Bookings;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingReservationServices(this IServiceCollection services)
    {
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<CreateBookingService>();
        services.AddScoped<GetBookingByIdService>();
        services.AddScoped<ConfirmPaymentService>();

        return services;
    }
}
