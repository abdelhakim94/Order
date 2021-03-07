using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Order.Server.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex.ToString(), ex);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                var errors = ex.Errors
                    .Select(e => new
                    {
                        e.Severity,
                        e.ErrorMessage,
                    });
                await context.Response.WriteAsync(JsonSerializer.Serialize(errors));
            }
            catch (BadRequestException ex)
            {
                logger.LogError(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status404NotFound;
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
