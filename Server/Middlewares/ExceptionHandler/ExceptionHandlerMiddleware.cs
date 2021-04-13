using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Order.Server.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex.ToString(), ex);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain";
                var error = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                await context.Response.WriteAsync(error);
            }
            catch (BadRequestException ex)
            {
                logger.LogError(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                logger.LogError(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (ApplicationException ex)
            {
                logger.LogError(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (InvalidEnumArgumentException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                logger.LogError(ex.ToString());
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                logger.LogError(ex.ToString());
            }
        }
    }
}
