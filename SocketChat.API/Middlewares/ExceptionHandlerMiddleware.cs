using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocketChat.Application.Exceptions;
using SocketChat.Domain.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SocketChat.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning($"Validation Exception occurred: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (AppException ex)
            {
                _logger.LogWarning($"Application Exception occurred: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"***Internal Unhandled Exception*** {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.HttpStatusCode;

            await context.Response.WriteAsync(new ValidationExceptionResponse()
            {
                StatusCode = context.Response.StatusCode,
                Type = "Validation Error",
                Message = exception.Message,
                Fields = exception.Fields,
            }.ToString());
        }

        private static async Task HandleExceptionAsync(HttpContext context, AppException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.HttpStatusCode;

            await context.Response.WriteAsync(new ExceptionResponse()
            {
                StatusCode = context.Response.StatusCode,
                Type = "Application Exception",
                Message = exception.Message,
            }.ToString());
        }

        private static async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new ExceptionResponse()
            {
                StatusCode = context.Response.StatusCode,
                Type = "Internal Server Error",
                Message = "An Internal Error has occurred. Please try again later.",
            }.ToString());
        }
    }

    public class ExceptionResponse
    {
        [JsonProperty(Order = -99)]
        public int StatusCode { get; set; }

        [JsonProperty(Order = -98)]
        public string Type { get; set; }

        [JsonProperty(Order = -97)]
        public string Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class ValidationExceptionResponse : ExceptionResponse
    {
        public IEnumerable<ValidationFieldErrors> Fields { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
