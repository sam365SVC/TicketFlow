using Booking.API.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Booking.API.Infrastructure.ExceptionHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is AppException appException)
        {
            httpContext.Response.StatusCode = appException.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(
                new ErrorResponse(appException.Message, appException.Error),
                cancellationToken);

            return true;
        }

        logger.LogError(exception, "Unhandled exception processing {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse("An unexpected error occurred.", "INTERNAL_SERVER_ERROR"),
            cancellationToken);

        return true;
    }
}
