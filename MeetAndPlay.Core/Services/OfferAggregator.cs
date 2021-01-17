using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;

namespace MeetAndPlay.Core.Services
{
    public class OfferAggregator : IOfferAggregator
    {
        private readonly ILobbyService _lobbyService;
        private readonly IUserOfferService _userOfferService;
        private readonly IPlaceService _placeService;

        public OfferAggregator(ILobbyService lobbyService, IUserOfferService userOfferService, IPlaceService placeService)
        {
            _lobbyService = lobbyService;
            _userOfferService = userOfferService;
            _placeService = placeService;
        }
        
        public async Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter)
        {
            var lobbies = await _lobbyService.AggregateOffersAsync(filter);
            var userOffers = await _userOfferService.AggregateOffersAsync(filter);
            var places = await _placeService.AggregateOffersAsync(filter);

            var allItems = lobbies.Items
                .Concat(userOffers.Items)
                .Concat(places.Items);

            var allCount = lobbies.Count + userOffers.Count + places.Count;

            return new CountArray<AggregatedOfferDto>(allItems.ToArray(), allCount);
        }
    }
}