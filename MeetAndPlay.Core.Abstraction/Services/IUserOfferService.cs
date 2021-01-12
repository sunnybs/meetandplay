using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Data.Models.Offers;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IUserOfferService : IReadService<UserOffer>, IOfferAggregator
    {
        
    }
}