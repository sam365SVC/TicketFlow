using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class InvalidCredentialsException()
    : AppException("Invalid username or password.", "INVALID_CREDENTIALS", StatusCodes.Status401Unauthorized);
