using Microsoft.AspNetCore.Diagnostics;

namespace Evento.Api.Exceptions;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> _logger;

    public AppExceptionHandler(
        ILogger<AppExceptionHandler> logger) {

        _logger = logger; }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
        
        (int statusCode, string errorMessage) = exception switch
        {
            
            //ForbidException => (403, null),
            BadHttpRequestException badHttpRequestException => (400, badHttpRequestException.Message),
            DllNotFoundException notFoundException => (404, notFoundException.Message),
            NotSupportedException notSupportedException => (405, notSupportedException.Message),
            //_ => (500, "Something went wrong")
            _ => default
        };
        if(statusCode == default)
        {
            return false;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(errorMessage);

        _logger.LogError(exception, exception.Message);

        return true;
    }
}
