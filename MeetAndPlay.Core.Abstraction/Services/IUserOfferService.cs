using System;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Data.Models.Offers;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IUserOfferService : IReadService<UserOffer>, IOfferAggregator
    {
        Task<Guid> AddUserOfferAsync(UserOffer userOffer);
        Task<Guid> UpdateUserOfferAsync(UserOffer userOffer);
    }
}