using Booking.API.Application.Abstractions;
using Booking.API.Application.Exceptions;
using Booking.Domain;
using TicketFlow.Shared.Events;
using TicketFlow.Shared.Models;

namespace Booking.API.Application.Payments;

/// <summary>
/// Confirma el pago de una reserva ya existente. No tiene controller propio: está pensado para que
/// lo invoque directamente quien resuelva la confirmación real del pago (gateway/webhook), sin exponer
/// un endpoint HTTP separado para esto.
/// </summary>
public class ConfirmPaymentService(IReservationRepository reservationRepository, IEventPublisher eventPublisher)
{
    /// <summary>
    /// Marca el pago de la reserva como autorizado y, si corresponde, la confirma:
    /// genera los tickets (uno por entrada comprada), descuenta el cupo vendido de cada zona,
    /// pasa la reserva a <see cref="BookingStatus.Confirmed"/> y publica <see cref="ReservationConfirmedEvent"/>
    /// para que el Notification.Worker le avise al cliente.
    /// </summary>
    /// <param name="bookingId">Id de la reserva a confirmar.</param>
    /// <param name="paymentMethod">Método de pago usado (ej. "credit_card", "yape", etc).</param>
    /// <param name="externalTransactionId">Id de transacción del proveedor de pago, si aplica.</param>
    /// <param name="cancellationToken">Token de cancelación de la operación.</param>
    /// <exception cref="BookingNotFoundException">No existe una reserva con ese id.</exception>
    /// <exception cref="InvalidBookingStateException">
    /// La reserva no está en <see cref="BookingStatus.Pending"/> (ya fue cancelada o falló antes).
    /// </exception>
    /// <remarks>
    /// Es idempotente: si la reserva ya está <see cref="BookingStatus.Confirmed"/>, no vuelve a generar
    /// tickets ni a publicar el evento de notificación; simplemente la devuelve tal cual. Esto es importante
    /// porque el llamador (confirmación de pago) puede reintentar la operación ante un timeout/retry.
    /// </remarks>
    public async Task<Reservation> ConfirmAsync(
        int bookingId,
        string paymentMethod,
        string? externalTransactionId,
        CancellationToken cancellationToken = default)
    {
        var reservation = await reservationRepository.GetByIdAsync(bookingId, cancellationToken);

        if (reservation is null)
        {
            throw new BookingNotFoundException();
        }

        if (reservation.Status == BookingStatus.Confirmed)
        {
            return reservation;
        }

        if (reservation.Status != BookingStatus.Pending)
        {
            throw new InvalidBookingStateException();
        }

        var now = DateTime.UtcNow;

        reservation.Payments.Add(new Payment
        {
            BookingId = reservation.Id,
            Status = PaymentStatus.Authorized,
            Amount = reservation.TotalAmount,
            PaymentMethod = paymentMethod,
            ExternalTransactionId = externalTransactionId,
            CreatedAt = now,
            UpdatedAt = now
        });

        var ticketInfos = new List<TicketInfo>();

        foreach (var item in reservation.BookingItems)
        {
            item.EventZone.TicketsSold += item.Quantity;

            for (var i = 0; i < item.Quantity; i++)
            {
                var ticket = new Ticket
                {
                    Status = TicketStatus.Valid,
                    TicketCode = Guid.NewGuid().ToString("N"),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                item.Tickets.Add(ticket);
                ticketInfos.Add(new TicketInfo
                {
                    TicketCode = ticket.TicketCode,
                    ZoneName = item.EventZone.TicketType.ToString()
                });
            }
        }

        reservation.Status = BookingStatus.Confirmed;
        reservation.UpdatedAt = now;

        await reservationRepository.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(new ReservationConfirmedEvent
        {
            CustomerName = reservation.User.FullName,
            CustomerEmail = reservation.User.Email,
            EventName = reservation.Event.Name,
            EventDate = reservation.Event.EventDateTime,
            Location = reservation.Event.Location,
            Tickets = ticketInfos
        }, cancellationToken);

        return reservation;
    }
}
