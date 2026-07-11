using Booking.API.Application.Abstractions;
using Booking.API.Application.Exceptions;
using Booking.Domain;

namespace Booking.API.Application.Bookings;

public class CreateBookingService(IEventRepository eventRepository, IReservationRepository reservationRepository)
{
    public async Task<Reservation> CreateAsync(CreateBookingRequest request, int userId, CancellationToken cancellationToken = default)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            throw new EventNotFoundException();
        }

        var zonesById = @event.EventZones.ToDictionary(z => z.Id);
        var now = DateTime.UtcNow;

        var bookingItems = new List<BookingItem>();
        decimal totalAmount = 0;

        foreach (var item in request.Items)
        {
            if (!zonesById.TryGetValue(item.EventZoneId, out var zone))
            {
                throw new InvalidEventZoneException();
            }

            var available = zone.Capacity - zone.TicketsSold;
            if (item.Quantity > available)
            {
                throw new InsufficientCapacityException();
            }

            totalAmount += zone.Price * item.Quantity;

            bookingItems.Add(new BookingItem
            {
                EventZoneId = zone.Id,
                EventZone = zone,
                Quantity = item.Quantity,
                UnitPrice = zone.Price,
                CreatedAt = now
            });
        }

        var reservation = new Reservation
        {
            EventId = @event.Id,
            Event = @event,
            UserId = userId,
            Status = BookingStatus.Pending,
            TotalAmount = totalAmount,
            CreatedAt = now,
            UpdatedAt = now,
            BookingItems = bookingItems
        };

        await reservationRepository.AddAsync(reservation, cancellationToken);
        await reservationRepository.SaveChangesAsync(cancellationToken);

        return reservation;
    }
}
