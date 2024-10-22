using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Extensions.Infrastrucure
{
    public class BadRequestExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BadRequestExceptionHandler> _logger;

        public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, 
            Exception ex,
            CancellationToken cancellationToken)
        {
            if (ex is not BadHttpRequestException badRequestException)
            {
                return false;
            }

            _logger.LogError(ex, "Exception occured: {ExceptionMessage}", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad request",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Detail = badRequestException.Message
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}