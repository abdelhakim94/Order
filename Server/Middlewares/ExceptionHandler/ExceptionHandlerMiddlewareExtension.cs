using Microsoft.AspNetCore.Builder;

namespace Order.Server.Middlewares
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
