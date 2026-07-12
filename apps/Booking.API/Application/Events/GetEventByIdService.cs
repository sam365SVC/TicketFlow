using Booking.API.Application.Abstractions;
using Booking.API.Application.Exceptions;
using Booking.Domain;

namespace Booking.API.Application.Events;

public class GetEventByIdService(IEventRepository eventRepository)
{
    public async Task<Event> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var @event = await eventRepository.GetByIdAsync(id, cancellationToken);

        if (@event is null)
        {
            throw new EventNotFoundException();
        }

        return @event;
    }
}
