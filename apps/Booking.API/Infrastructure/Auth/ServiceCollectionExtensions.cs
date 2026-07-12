using Booking.API.Application.Abstractions;
using Booking.API.Application.Auth;
using Booking.API.Infrastructure.Repositories;

namespace Booking.API.Infrastructure.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<AuthService>();

        return services;
    }
}
