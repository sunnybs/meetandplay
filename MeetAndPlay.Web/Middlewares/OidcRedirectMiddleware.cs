using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace MeetAndPlay.Web.Middlewares
{
    public class OidcRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        public OidcRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.ToString();
            if (path.Contains("auth_endpoint"))
            {
                if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    httpContext.Request.Query.TryGetValue("returnUrl", out var returnUrl);
                    if (string.IsNullOrEmpty(returnUrl))
                        returnUrl = "/";
                    
                    httpContext.Response.StatusCode = 301;
                    httpContext.Response.Headers.Add("Location", returnUrl);
                }
                else
                {
                    await httpContext.ChallengeAsync();
                }
                return;
            }
            await _next(httpContext);
        }

    }
}