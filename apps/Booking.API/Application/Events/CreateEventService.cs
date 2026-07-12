using Booking.API.Application.Abstractions;
using Booking.API.Application.Exceptions;
using Booking.Domain;

namespace Booking.API.Application.Events;

public class CreateEventService(IEventRepository eventRepository)
{
    public async Task<Event> CreateAsync(CreateEventRequest request, int createdByUserId, CancellationToken cancellationToken = default)
    {
        var hasDuplicateZones = request.Zones
            .GroupBy(z => z.TicketType)
            .Any(g => g.Count() > 1);

        if (hasDuplicateZones)
        {
            throw new DuplicateEventZoneException();
        }

        var now = DateTime.UtcNow;

        var @event = new Event
        {
            CreatedBy = createdByUserId,
            Name = request.Name,
            Description = request.Description,
            Location = request.Location,
            EventDateTime = request.EventDateTime,
            ImageUrl = request.ImageUrl,
            IsPublished = request.IsPublished,
            CreatedAt = now,
            UpdatedAt = now,
            EventZones = request.Zones.Select(z => new EventZone
            {
                TicketType = z.TicketType,
                Price = z.Price,
                Capacity = z.Capacity,
                TicketsSold = 0,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList()
        };

        await eventRepository.AddAsync(@event, cancellationToken);
        await eventRepository.SaveChangesAsync(cancellationToken);

        return @event;
    }
}
