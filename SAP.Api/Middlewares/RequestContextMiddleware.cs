using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SAP.Domain.Constants;
using SAP.Domain.Dtos;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAP.Api.Middlewares
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRequestContext requestContext)
        {
            var nameIdentifier = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var permissions = context.User.Claims
                .Where(c => c.Type == CustomsClaimTypes.SapPermission)
                .Select(c => c.Value)
                .ToArray();

            requestContext.UserId = nameIdentifier?.Value;
            requestContext.PermissionClaims = permissions;

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class RequestContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestContext(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestContextMiddleware>();
        }
    }
}
