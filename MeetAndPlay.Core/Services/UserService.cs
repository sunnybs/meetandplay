using System;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAuthenticationService _userAuthentication;
        private readonly DbContextFactory _contextFactory;

        public UserService(IUserAuthenticationService userAuthentication, DbContextFactory contextFactory)
        {
            _userAuthentication = userAuthentication;
            _contextFactory = contextFactory;
        }
        
        public async Task<User> GetCurrentUserAsync()
        {
            var login = _userAuthentication.GetCurrentUserName();
            await using var mnpContext = _contextFactory.Create();
            
            var user = await mnpContext.Users
                .IncludeImagesAndRoles()
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName.ToLower() == login.ToLower());

            return user;
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var login = _userAuthentication.GetCurrentUserName();
            await using var mnpContext = _contextFactory.Create();
            
            var userId = await mnpContext.Users
                .Where(u => u.UserName.ToLower() == login.ToLower())
                .AsNoTracking()
                .Select(u => u.Id)
                .SingleAsync();

            return userId;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var user = await mnpContext.Users
                .IncludeImagesAndRoles()
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByLoginAsync(string userName)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var user = await mnpContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .Include(u => u.UserImages)
                .ThenInclude(u => u.File)
                .Include(u => u.UserImages)
                .ThenInclude(u => u.CompressedFile)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());

            if (user == null)
                throw new NotFoundException($"user with login {userName} not found");

            return user;
        }
        
        
    }
}