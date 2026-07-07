using Microsoft.AspNetCore.Http;

namespace Booking.API.Application.Exceptions;

public class UserAlreadyExistsException()
    : AppException("A user with that username or email already exists.", "USER_ALREADY_EXISTS", StatusCodes.Status409Conflict);
