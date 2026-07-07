namespace Booking.API.Application.Exceptions;

public abstract class AppException : Exception
{
    public string Error { get; }
    public int StatusCode { get; }

    protected AppException(string message, string error, int statusCode) : base(message)
    {
        Error = error;
        StatusCode = statusCode;
    }
}
