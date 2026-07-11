using Booking.API.Application.Abstractions;
using Booking.API.Application.Exceptions;
using Booking.Domain;

namespace Booking.API.Application.Bookings;

public class GetBookingByIdService(IReservationRepository reservationRepository)
{
    /// <summary>
    /// Trae una reserva por id. Si <paramref name="requestingUserId"/> no es el dueño de la reserva
    /// y <paramref name="requestingUserRole"/> es menor a <see cref="UserRole.Staff"/>, se trata como no encontrada
    /// (no se revela que la reserva existe a quien no tiene permiso de verla).
    /// </summary>
    public async Task<Reservation> GetAsync(int id, int requestingUserId, UserRole requestingUserRole, CancellationToken cancellationToken = default)
    {
        var reservation = await reservationRepository.GetByIdAsync(id, cancellationToken);

        if (reservation is null || (reservation.UserId != requestingUserId && requestingUserRole < UserRole.Staff))
        {
            throw new BookingNotFoundException();
        }

        return reservation;
    }
}
