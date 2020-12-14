using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;

namespace MeetAndPlay.Core.Services
{
    public class UserOffersService
    {
        private readonly MNPContext _mnpContext;
        private readonly IUserService _userService;

        public UserOffersService(MNPContext mnpContext, IUserService userService)
        {
            _mnpContext = mnpContext;
            _userService = userService;
        }
    }
}