using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class UserNotFoundException() : AppException("User not found.", "USER_NOT_FOUND", StatusCodes.Status404NotFound);
