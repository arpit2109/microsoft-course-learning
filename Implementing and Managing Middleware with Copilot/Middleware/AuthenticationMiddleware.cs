using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Validating the authentication token
            // For example purposes, we expect a 'Valid-Token' string in the Authorization header.
            if (!context.Request.Headers.TryGetValue("Authorization", out var extractedToken) || 
                extractedToken != "Bearer Valid-Token")
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
                return;
            }

            // Valid Token -> Proceed in pipeline
            await _next(context);
        }
    }
}
