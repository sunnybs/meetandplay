using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetAndPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [HttpGet("ExecuteSeeding")]
        public async Task ExecuteSeeding()
        {
            await _gamesService.SeedGamesAsync();
        }
    }
}