using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Extensions.Infrastrucure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> loggger)
        {
            _logger = loggger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, 
            Exception ex,
            CancellationToken cancellationToken)
        {
            if (ex is not Exception exception)
            {
                return false;
            }

            _logger.LogError(ex, "Exception occured: {ExceptionMessage}", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }        
    }
}