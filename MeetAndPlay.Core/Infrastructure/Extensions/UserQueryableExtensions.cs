using System.Linq;
using MeetAndPlay.Data.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Infrastructure.Extensions
{
    public static class UserQueryableExtensions
    {
        public static IQueryable<User> IncludeImagesAndRoles(this IQueryable<User> users)
        {
            return users
                .Include(u => u.UserImages).ThenInclude(i => i.File)
                .Include(u => u.UserRoles).ThenInclude(r => r.Role);
        }
    }
}