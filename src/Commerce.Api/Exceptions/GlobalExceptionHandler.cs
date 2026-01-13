using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Commerce.Application.Exceptions;

namespace Commerce.Api.Exceptions;

internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger
    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occured");

        var (status, title, type) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Bad Request", "https://httpstatuses.com/400"),
            NotFoundException   => (StatusCodes.Status404NotFound, "Not Found", "https://httpstatuses.com/404"),
            ConflictException   => (StatusCodes.Status409Conflict, "Conflict", "https://httpstatuses.com/409"),
            ForbiddenException  => (StatusCodes.Status403Forbidden, "Forbidden", "https://httpstatuses.com/403"),
            ExternalServiceException => (StatusCodes.Status503ServiceUnavailable, "Service Unavailable", "https://httpstatuses.com/503"),
            _                   => (StatusCodes.Status500InternalServerError, "Internal Server Error", "https://httpstatuses.com/500")
        };
        if (status >= 500)
            logger.LogError(exception, "Unhandled exception");
        else
            logger.LogWarning(exception, "Request failed with {StatusCode}", status);

        httpContext.Response.StatusCode = status;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Type = type,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            }.Also(pd => pd.Extensions["traceId"] = httpContext.TraceIdentifier)
        });
    }
}

internal static class ProblemDetailsExtensions
{
    public static T Also<T>(this T obj, Action<T> action)
    {
        action(obj);
        return obj;
    }
}