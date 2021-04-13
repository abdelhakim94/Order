using Microsoft.AspNetCore.Builder;

namespace Order.Server.Middlewares
{
    public static class ExternalProviderSigninRedirectionMiddlewareExtension
    {
        public static IApplicationBuilder UseExternalProviderSigninRedirection(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExternalProviderSigninRedirectionMiddleware>();
            return app;
        }
    }
}
