using System;
using System.Security.Claims;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace MeetAndPlay.Core.Services
{
    public class CookieUserAuthenticationService : IUserAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieUserAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                throw new NullReferenceException(nameof(context));
            
            if (!IsAuthenticated())
                throw new NoUserContextException($"Request {context.Request.Path} require user authentication");
            
            var loginClaim = context.User.FindFirst("name");
            if (loginClaim == null)
                throw new NoUserContextException($"Request {context.Request.Path} require user authentication");

            return loginClaim.Value;
        }

        public bool IsAuthenticated()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}