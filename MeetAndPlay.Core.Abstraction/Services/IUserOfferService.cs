using System;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Data.Models.Offers;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IUserOfferService : IReadService<UserOffer>, IOfferAggregator
    {
        Task<UserOffer> GetByUserNameAsync(string username);
        Task<Guid> AddUserOfferAsync(UserOffer userOffer);
        Task<Guid> UpdateUserOfferAsync(UserOffer userOffer);
        Task<UserOffer> GetOfferByUserIdAsync(Guid userId);
    }
}