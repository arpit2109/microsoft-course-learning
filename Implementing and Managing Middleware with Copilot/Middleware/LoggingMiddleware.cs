using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the HTTP method and request path before continuing in the pipeline
            var method = context.Request.Method;
            var path = context.Request.Path;

            // Wait for the response pipeline to complete
            await _next(context);
            
            // Extract the response status code
            var statusCode = context.Response.StatusCode;

            // Log the captured details
            _logger.LogInformation($"HTTP Request {method} {path} completed with Status Code: {statusCode}");
        }
    }
}
