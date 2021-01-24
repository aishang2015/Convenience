using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Convience.Util.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next,
            ILogger<CustomExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var problem = new
                {
                    detail = ex.Message
                };
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problem);
            }
        }
    }
}
