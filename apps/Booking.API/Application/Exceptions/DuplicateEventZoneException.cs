using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class DuplicateEventZoneException()
    : AppException("An event cannot have two zones of the same ticket type.", "DUPLICATE_EVENT_ZONE", StatusCodes.Status400BadRequest);
