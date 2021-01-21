using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace MeetAndPlay.API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{login}")]
        public async Task<User> Get([FromRoute] string login)
        {
            return await _userService.GetUserByLoginAsync(login);
        }
    }
}