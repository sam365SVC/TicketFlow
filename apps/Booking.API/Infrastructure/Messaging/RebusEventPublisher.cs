using Booking.API.Application.Abstractions;
using Rebus.Bus;

namespace Booking.API.Infrastructure.Messaging;

public class RebusEventPublisher(IBus bus) : IEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
        => bus.Send(@event);
}
