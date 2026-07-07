namespace Booking.API.Infrastructure.ExceptionHandling;

/// <summary>
/// Standard error shape returned for every handled/unhandled exception.
/// </summary>
/// <param name="Message">Human-readable description of what went wrong.</param>
/// <param name="Error">Stable machine-readable code in SCREAMING_SNAKE_CASE (e.g. <c>USER_NOT_FOUND</c>), meant for the frontend to branch on without parsing <paramref name="Message"/>.</param>
public record ErrorResponse(string Message, string Error);
