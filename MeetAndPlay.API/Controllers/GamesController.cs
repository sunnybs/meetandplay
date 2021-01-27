using System.Collections.Generic;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Data.Models.Games;
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

        [HttpGet]
        public async Task<IReadOnlyList<Game>> GetAll()
        {
            return await _gamesService.GetAsync(new ReadFilter());
        }
    }
}