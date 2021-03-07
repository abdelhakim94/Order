using Microsoft.AspNetCore.Builder;

namespace Order.Server.Exceptions
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseResponseExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            return app;
        }
    }
}
