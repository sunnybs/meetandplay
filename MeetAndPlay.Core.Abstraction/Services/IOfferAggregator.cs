using System.Threading.Tasks;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IOfferAggregator
    {
        Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter);
    }
}