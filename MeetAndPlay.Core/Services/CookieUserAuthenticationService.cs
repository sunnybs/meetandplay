using System;
using System.Security.Claims;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace MeetAndPlay.Core.Services
{
    public class CookieUserAuthenticationService : IUserAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NavigationManager _navigationManager;

        public CookieUserAuthenticationService(IHttpContextAccessor httpContextAccessor, NavigationManager navigationManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _navigationManager = navigationManager;
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

        public void Challenge()
        {
            _navigationManager.NavigateTo($"/auth_endpoint?returnUrl={_navigationManager.Uri}",true);
        }
    }
}