using Booking.API.Application.Abstractions;
using Booking.Domain;

namespace Booking.API.Application.Events;

public class GetPublishedEventsService(IEventRepository eventRepository)
{
    public Task<IReadOnlyList<Event>> GetAsync(CancellationToken cancellationToken = default) =>
        eventRepository.GetPublishedAsync(cancellationToken);
}
