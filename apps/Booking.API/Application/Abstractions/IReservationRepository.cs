using Booking.Domain;

namespace Booking.API.Application.Abstractions;

public interface IReservationRepository
{
    /// <summary>
    /// Trae una reserva por id, con su evento, usuario, items (con su zona) y pagos incluidos.
    /// </summary>
    Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
