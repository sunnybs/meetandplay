using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IGamesService : IReadService<Game>
    {
        Task SeedGamesAsync();
    }
}