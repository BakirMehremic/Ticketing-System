using System.Text.Json;
using TicketingSys.Exceptions;

namespace TicketingSys.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IWebHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogWarning("Unhandled exception caught in ExceptionHandlingMiddleware:" +
                               " {ExceptionName}", ex.GetType().Name);
            await HandleExceptionAsync(context, ex);
        }
    }

    // this handles custom exceptions thrown frequently and removing stack traces from unexpected errors in prod
    // custom exceptions which are not thrown often are handled near where they are thrown
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;
        string? details = null;

        switch (exception)
        {
            case NoUserIdInJwtException:
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Missing userId - sub in JWT.";
                break;

            case ForbiddenException forbiddenEx:
                statusCode = StatusCodes.Status403Forbidden;
                message = forbiddenEx.Message;
                break;

            default:
                logger.LogWarning("Unexpected error occurred: "+ exception.Message);
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Internal Server Error";
                break;
        }

        if (env.IsDevelopment())
        {
            details = exception.ToString();
        }

        var response = new
        {
            status = statusCode,
            message,
            detail = details
        };

        context.Response.StatusCode = statusCode;
        string json = JsonSerializer.Serialize(response);

        logger.LogInformation("Returning status code from ExceptionHandlingMiddleware: " +
                               context.Response.StatusCode);

        return context.Response.WriteAsync(json);
    }
}