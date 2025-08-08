using System.Diagnostics;
using System.Text.Json;

using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Middleware
{
    public class ErrorExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleGenericExceptionAsync(context, ex, StatusCodes.Status401Unauthorized);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleGenericExceptionAsync(context, ex, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Request error");
                await HandleGenericExceptionAsync(context, new Exception("Internal Server Error", ex), StatusCodes.Status500InternalServerError);
            }
        }

        private Task HandleGenericExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiResponse
            {
                Success = false,
                Message = exception.Message,
            };

            if (Debugger.IsAttached || _env.EnvironmentName == "Test")
                response.Message = exception.ToString();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
