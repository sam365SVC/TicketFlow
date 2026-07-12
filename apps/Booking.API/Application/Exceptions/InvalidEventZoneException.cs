using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class InvalidEventZoneException()
    : AppException("One or more selected zones do not belong to the specified event.", "INVALID_EVENT_ZONE", StatusCodes.Status400BadRequest);
