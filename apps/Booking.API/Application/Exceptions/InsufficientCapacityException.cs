using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class InsufficientCapacityException()
    : AppException("There are not enough tickets available in one or more selected zones.", "INSUFFICIENT_CAPACITY", StatusCodes.Status400BadRequest);
