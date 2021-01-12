using System;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAuthenticationService _userAuthentication;
        private readonly MNPContext _mnpContext;

        public UserService(IUserAuthenticationService userAuthentication, MNPContext mnpContext)
        {
            _userAuthentication = userAuthentication;
            _mnpContext = mnpContext;
        }
        
        public async Task<User> GetCurrentUserAsync()
        {
            var login = _userAuthentication.GetCurrentUserName();
            var user = await _mnpContext.Users
                .IncludeImagesAndRoles()
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName.ToLower() == login.ToLower());

            return user;
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var login = _userAuthentication.GetCurrentUserName();
            var userId = await _mnpContext.Users
                .Where(u => u.UserName.ToLower() == login.ToLower())
                .AsNoTracking()
                .Select(u => u.Id)
                .SingleAsync();

            return userId;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _mnpContext.Users
                .IncludeImagesAndRoles()
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }
        
        
    }
}