using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class EventNotFoundException() : AppException("Event not found.", "EVENT_NOT_FOUND", StatusCodes.Status404NotFound);
