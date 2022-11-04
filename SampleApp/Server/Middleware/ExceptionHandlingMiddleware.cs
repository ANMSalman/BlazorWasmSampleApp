using SampleApp.Server.Application.Exceptions;
using SampleApp.Server.Domain.Exceptions;
using SampleApp.Shared.ResponseModels.Error;
using System.Net;

namespace SampleApp.Server.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (DomainException ex)
        {
            await HandleUserExceptionAsync(httpContext, ex);
        }
        catch (AppException ex)
        {
            await HandleUserExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorDetails = new ErrorDetailsResponseModel()
        {
            Message = "Internal Server Error. Please try again in a moment"
        };

        await context.Response.WriteAsync(errorDetails.ToString());
    }
    private async Task HandleUserExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errorDetails = new ErrorDetailsResponseModel()
        {
            Message = exception.Message
        };

        await context.Response.WriteAsync(errorDetails.ToString());
    }
}

