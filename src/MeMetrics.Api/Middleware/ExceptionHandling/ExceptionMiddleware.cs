using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using MeMetrics.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;

namespace MeMetrics.Api.Middleware.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        // https://stackoverflow.com/a/38935583/856692
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IOptions<EnvironmentConfiguration> _configuration;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger logger,
            IOptions<EnvironmentConfiguration> configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.Error(exception, exception.Message);

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            if (exception is ValidationException) code = HttpStatusCode.BadRequest;
            context.Response.StatusCode = (int)code;

            string result = "An error has occurred";
            if (_configuration.Value.ENV.ToLower() == "dev")
            {
                result = JsonSerializer.Serialize(new
                {
                    error = exception.Message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries),
                    stack = exception.StackTrace.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                });
                context.Response.ContentType = "application/json";
            }

            return context.Response.WriteAsync(result);
        }
    }
}
