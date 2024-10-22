using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Extensions.Infrastrucure
{
    public class UnauthorizedExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<UnauthorizedExceptionHandler> _logger;

        public UnauthorizedExceptionHandler(ILogger<UnauthorizedExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, 
            Exception ex,
            CancellationToken cancellationToken)
        {
            if (ex is not UnauthorizedAccessException unauthorizedAccessException)
            {
                return false;
            }

            _logger.LogError(ex, "Exception occured: {ExceptionMessage}", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Detail = unauthorizedAccessException.Message
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}