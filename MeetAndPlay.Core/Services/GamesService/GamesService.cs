using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.Models.Games;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services.GamesService
{
    public class GamesService : IGamesService
    {
        private readonly MNPContext _mnpContext;
        private readonly IFilesService _filesService;

        public GamesService(MNPContext mnpContext, IFilesService filesService)
        {
            _mnpContext = mnpContext;
            _filesService = filesService;
        }

        public async Task SeedGamesAsync()
        {
            var httpClient = new HttpClient();
            for (var i = 0; i <= 300; i += 100)
            {
                var teseraGames =
                    await httpClient.GetFromJsonAsync<GameTeseraResource[]>(
                        $"https://api.tesera.ru/games?offset={i}&limit=100");

                teseraGames = teseraGames.Where(g => g.TeseraId.ToString() != g.Title).ToArray();
                foreach (var teseraGame in teseraGames)
                {
                    if (await _mnpContext.Games.AnyAsync(g => g.Name == teseraGame.Title))
                        continue;

                    var game = teseraGame.ToGame();
                    await _mnpContext.Games.AddAsync(game);
                    //TODO: Implement file upload;
                }
            }

            await _mnpContext.SaveChangesAsync();
        }

        public Task<CountArray<NamedEntityDto>> GetNamedEntitiesAsync(ReadFilterDto filter)
        {
            throw new System.NotImplementedException();
        }
    }
}