using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MeetAndPlay.Web.Infrastructure
{
    public class CoreAuthenticationStateProvider: RevalidatingServerAuthenticationStateProvider

    {
        private readonly IServiceScopeFactory _scopeFactory;

        private const string IdClaim = "sub";
        private const string AuthTimeClaim = "auth_time";
        
        public CoreAuthenticationStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory) : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope();
            try
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                return await ValidateSecurityStampAsync(userService, authenticationState.User);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);
        
        private async Task<bool> ValidateSecurityStampAsync(IUserService userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserByIdAsync(Guid.Parse(principal.FindFirstValue(IdClaim)));
            if (user == null)
                return false;
            
            var authTime = long.Parse(principal.FindFirstValue(AuthTimeClaim));
            return user.LastCredentialsChangeDate <= authTime.FromUnixTime();
        }
    }
}