using System.Net;
using System.Text.Json;
using log4net;
using MovieApi.Exceptions;

namespace MovieApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILog _log = LogManager.GetLogger(typeof(ExceptionHandlerMiddleware));

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _log.Error("error during executing {Context}", ex);
                await ExceptionHandlerAsync(context, ex);
            }
        }

        private Task ExceptionHandlerAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError; // Default 500
            string message = "An unexpected error occurred";
            int? errorCode = null;
            var errorrs = new List<string>();

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401
                    message = "Unauthorized access";
                    break;

                case ForbiddenAccessException:
                    statusCode = (int)HttpStatusCode.Forbidden; // 403
                    message = "Forbidden access";
                    break;

                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    message = exception.Message;
                    break;

                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    message = exception.Message;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError; // 500
                    message = "An unexpected error occurred";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                success = false,
                error_code = errorCode,
                errors = errorrs ?? [],
                message = message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}