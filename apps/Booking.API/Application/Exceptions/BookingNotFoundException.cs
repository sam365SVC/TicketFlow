using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class BookingNotFoundException() : AppException("Booking not found.", "BOOKING_NOT_FOUND", StatusCodes.Status404NotFound);
