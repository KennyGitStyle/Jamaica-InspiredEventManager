using System.Runtime.Serialization;
using System.Net;
using System;
using System.Threading.Tasks;
using Jam_Inspired_Application.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Jam_Inspired.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext) {
            try 
            {
                await _next(httpContext);

            }catch (Exception ex)
            {
                await HandleExeptionAsync(httpContext, ex, _logger);
            }
            
        }

        private async Task HandleExeptionAsync(HttpContext httpContext, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            switch (ex)
            {
                case RestExceptionHandling re:
                _logger.LogError(ex.Message);
                errors = re.Error;
                httpContext.Response.StatusCode = Convert.ToInt32(re.Status);
                break;

                case Exception e:
                _logger.LogError(e.Message);
                errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                break;

                default:
                _logger.LogError(ex.Message);
                break;
            }

            httpContext.Response.ContentType = "application/json";

            if(errors != null) 
            {
                var result = JsonSerializer.Serialize(new { errors });
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}