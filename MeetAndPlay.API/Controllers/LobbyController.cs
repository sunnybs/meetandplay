using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO.Lobies;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Data.Models.Games;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.AspNetCore.Mvc;

namespace MeetAndPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public LobbyController(ILobbyService lobbyService, IGamesService gamesService, IMapper mapper)
        {
            _lobbyService = lobbyService;
            _gamesService = gamesService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<Guid> AddAsync(MobileLobbyAdd addModel)
        {
            var gamesNames = addModel.NameOfTheGame.Split(", ");
            var gameList = new List<Game>();
            foreach (var gameName in gamesNames)
            {
                var gamesFoundByName = await _gamesService.GetAsync(new ReadFilter {SearchTerm = gameName});
                gameList.AddRange(gamesFoundByName);
            }
            //TODO: implement distinct

            var lobbyDomain = _mapper.Map<Lobby>(addModel);
            var result = await _lobbyService.AddLobbyAsync(lobbyDomain);
            return result;
        }
    }
}