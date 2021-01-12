using System.Threading.Tasks;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IPlaceService : IOfferAggregator
    {
        Task ExecuteSeeding();
    }
}