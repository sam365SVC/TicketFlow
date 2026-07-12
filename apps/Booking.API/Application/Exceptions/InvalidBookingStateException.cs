using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class InvalidBookingStateException()
    : AppException("The booking is not in a state that allows this operation.", "INVALID_BOOKING_STATE", StatusCodes.Status400BadRequest);
