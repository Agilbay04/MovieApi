using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Extensions.Infrastrucure
{
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, 
            Exception ex,
            CancellationToken cancellationToken)
        {
            if (ex is not DllNotFoundException notFoundException)
            {
                return false;
            }

            _logger.LogError(ex, "Exception occured: {ExceptionMessage}", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not found",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Detail = notFoundException.Message,
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}