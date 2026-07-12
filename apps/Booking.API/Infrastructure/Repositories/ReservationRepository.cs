using Booking.API.Application.Abstractions;
using Booking.Domain;
using Booking.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Infrastructure.Repositories;

public class ReservationRepository(BookingDbContext dbContext) : IReservationRepository
{
    public Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => dbContext.Reservations
            .Include(r => r.Event)
            .Include(r => r.User)
            .Include(r => r.Payments)
            .Include(r => r.BookingItems).ThenInclude(i => i.EventZone)
            .Include(r => r.BookingItems).ThenInclude(i => i.Tickets)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
        => await dbContext.Reservations.AddAsync(reservation, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
