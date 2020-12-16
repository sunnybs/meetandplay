using System;
using System.Threading.Tasks;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
        Task<Guid> GetCurrentUserIdAsync();
        Task<User> GetUserByIdAsync(Guid id);
    }
}