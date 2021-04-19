using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Order.Server.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public LoggingMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, ILogger<LoggingMiddleware> logger)
        {
            logger.LogDebug($"Response to request: '{context.Request.Path}' started. Protocol: '{context.Request.Protocol}'. Scheme: '{context.Request.Scheme}'. Method: '{context.Request.Method}'. Query-string: '{context.Request.QueryString}'. Content-type: '{context.Request.ContentType}'.");
            await requestDelegate.Invoke(context);
        }
    }
}
