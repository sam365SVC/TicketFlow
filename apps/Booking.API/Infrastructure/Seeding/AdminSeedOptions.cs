namespace Booking.API.Infrastructure.Seeding;

/// <summary>
/// Credenciales del usuario root Admin que se crea al arrancar la app (ver <see cref="AdminUserSeeder"/>).
/// Se leen de configuración (sección <see cref="SectionName"/>) para poder pisarlas por variable de
/// entorno (ej. <c>AdminSeed__Password</c> en docker-compose / .env) sin tocar código.
/// </summary>
public class AdminSeedOptions
{
    public const string SectionName = "AdminSeed";

    public string Username { get; set; } = "admin";
    public string Email { get; set; } = "admin@ticketflow.local";
    public string Password { get; set; } = "admin";
}
