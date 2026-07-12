using Booking.API.Application.Abstractions;
using Booking.Domain;
using Booking.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Infrastructure.Repositories;

public class EventRepository(BookingDbContext dbContext) : IEventRepository
{
    public Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => dbContext.Events
            .Include(e => e.EventZones)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Event>> GetPublishedAsync(CancellationToken cancellationToken = default)
        => await dbContext.Events
            .Include(e => e.EventZones)
            .Where(e => e.IsPublished)
            .OrderBy(e => e.EventDateTime)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Event @event, CancellationToken cancellationToken = default)
        => await dbContext.Events.AddAsync(@event, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
