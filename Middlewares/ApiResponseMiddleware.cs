using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MovieApi.Responses;

namespace MovieApi.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();

            context.Response.Body = responseBody;

            await _next(context);

            context.Response.ContentType = "application/json";
        
            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var jsonData = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                context.Response.Body.SetLength(0);

                var baseResponse = new BaseResponse<object>
                {
                    Success = true,
                    Message = "Request successful",
                    Data = jsonData
                };

                await context.Response.WriteAsJsonAsync(baseResponse);
            }
            else
            {
                var baseResponse = new BaseResponse<object>
                {
                    Success = false,
                    Message = "An error occurred",
                    Data = null
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(baseResponse);
            }
        }
    }
}