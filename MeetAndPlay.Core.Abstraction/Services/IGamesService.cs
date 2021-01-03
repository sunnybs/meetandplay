using System.Threading.Tasks;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IGamesService : INamedEntitiesService
    {
        Task SeedGamesAsync();
    }
}