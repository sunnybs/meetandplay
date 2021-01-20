using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MeetAndPlay.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MeetAndPlay.API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            
        }

        [HttpGet]
        public async Task<User> Get()
        {
            return new User
            {
                FirstName = "Тест",
                LastName = "Тест",
                AboutUser = "Тест"
            };
        }
    }
    
    public class User
    {
        //public ImageSource UserAvatar { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string AboutUser { get; set; }


    
        public string FreeTime { get; set; }
        public string UserGames { get; set; }
        public string AboutUserGamesInterests { get; set; }

        
        public string UserSocialNetworks { get; set; }
        public string UserEmail { get; set; }
        //public ObservableCollection<RequestViewModel> UserRequests { get; set; }
    }
}