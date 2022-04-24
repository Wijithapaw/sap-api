using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAP.Domain.Dtos;
using SAP.Domain.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SAP.Api.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            IRequestContext requestContext,
            ILogger<ErrorLoggingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorMessage = "";
                var responseStatus = HttpStatusCode.InternalServerError;
                if (ex is SapException)
                {
                    logger.LogWarning(ex, $"Bad Request. UserId: { requestContext.UserId}");
                    errorMessage = ex.Message;
                    responseStatus = HttpStatusCode.BadRequest;
                }
                else
                {
                    logger.LogError(ex, $"Unhandled Error. UserId: { requestContext.UserId}");
                }

                if (string.IsNullOrEmpty(errorMessage))
                    errorMessage = "ERR_UNKNOWN_ERROR";

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)responseStatus;
                await context.Response.WriteAsync(errorMessage);
                return;
            }
        }
    }

    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLogging(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
