using Booking.API.Application.Abstractions;
using Booking.Domain;
using Microsoft.Extensions.Options;

namespace Booking.API.Infrastructure.Seeding;

/// <summary>
/// Crea el usuario root Admin al arrancar la aplicación, si todavía no existe.
/// Corre en cada arranque (incluido cada <c>docker compose up</c>) y es idempotente:
/// si el usuario ya existe, no hace nada. Las credenciales salen de <see cref="AdminSeedOptions"/>
/// (configurables por variable de entorno, ver <c>AdminSeed__*</c> en docker-compose.yml).
/// </summary>
public static class AdminUserSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<AdminSeedOptions>>().Value;

        // La DB puede no estar lista todavía si booking-api arranca antes que Postgres.
        const int maxAttempts = 5;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var existingAdmin = await userRepository.GetByUsernameAsync(options.Username, cancellationToken);
                if (existingAdmin is not null)
                {
                    logger.LogInformation("Usuario {Username} ya existe, no se vuelve a crear.", options.Username);
                    return;
                }

                var admin = new User
                {
                    FullName = "Administrador",
                    Username = options.Username,
                    Email = options.Email,
                    PasswordHash = passwordHasher.Hash(options.Password),
                    Role = UserRole.Admin,
                    Active = true,
                    RequiresPwdChange = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await userRepository.AddAsync(admin, cancellationToken);
                await userRepository.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Usuario {Username} creado (seed inicial).", options.Username);
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                logger.LogWarning(ex, "No se pudo verificar/crear el usuario admin (intento {Attempt}/{MaxAttempts}). Reintentando...", attempt, maxAttempts);
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
    }
}
