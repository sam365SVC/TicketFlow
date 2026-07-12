namespace Booking.Domain;

// El orden importa: se usa para comparar jerarquía de permisos (Admin >= Staff >= Customer).
// El valor guardado en la DB sigue siendo el nombre como texto (VARCHAR + CHECK), no el número.
public enum UserRole
{
    Customer,
    Staff,
    Admin
}
