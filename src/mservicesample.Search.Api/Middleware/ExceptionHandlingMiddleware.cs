using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using mservicesample.Search.Api.Dtos.Responses.Global;
using mservicesample.Search.Api.Helpers;
using Newtonsoft.Json;

namespace mservicesample.Search.Api.Middleware
{
      public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context); //continue
            }
            catch (Exception ex)
            {
                var message = CreateMessage(context, ex);
                _logger.LogError(ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var rs = new AppResultModel() { IsSuccessful = false, Message = ex.Message };
            int statusCode;

            if (ex is ArgumentException || ex is ArgumentNullException)
            {
                statusCode = StatusCodes.Status400BadRequest;
            }
            else if (ex is AppException)
            {
                statusCode = StatusCodes.Status422UnprocessableEntity;
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                rs.Message = "Unknown error, please contact the system admin";
            }

            _logger.LogError(ex.Message);

            var response = JsonConvert.SerializeObject(rs, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(response);
        }

        private string CreateMessage(HttpContext context, Exception e)
        {
            var message = $"Exception caught in global error handler, exception message: {e.Message}, exception stack: {e.StackTrace}";

            if (e.InnerException != null)
            {
                message = $"{message}, inner exception message {e.InnerException.Message}, inner exception stack {e.InnerException.StackTrace}";
            }

            return $"{message} RequestId: {context.TraceIdentifier}";
        }
    }
}
