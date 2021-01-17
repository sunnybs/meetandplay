using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Web.Options;
using MeetAndPlay.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MeetAndPlay.Web.Components.Category
{
    public class BrowserComponent : ComponentBase
    {
        [CascadingParameter] public OffersFilterDto FilterModel { get; set; }
        [Parameter] public IOfferAggregator OfferAggregator { get; set; }
        
        [Inject] public IOptions<ApiInfo> ApiInfo { get; set; }
        [Inject] public IApiClient ApiClient { get; set; }
        
        protected IEnumerable<AggregatedOfferDto> CurrentOffers { get; set; } = new List<AggregatedOfferDto>();
        protected PagingFilter CurrentPage { get; set; } = new ();
        protected const int PageSize = 100;

        protected override async Task OnParametersSetAsync()
        {
            CurrentPage.PageSize = PageSize;
            CurrentPage.PageNumber = 1;
            
            FilterModel.StateHasChanged += async () =>
            {
                await UpdateOffersAsync(CurrentPage);
                StateHasChanged();
            };
            await UpdateOffersAsync(CurrentPage);
        }

        private async Task UpdateOffersAsync(PagingFilter filter)
        {
            FilterModel.PageSize = filter.PageSize;
            FilterModel.PageNumber = filter.PageNumber;
            
            var offers = await OfferAggregator.AggregateOffersAsync(FilterModel);

            foreach (var item in offers.Items)
            {
                item.PosterUrl = await GetPosterAsync(item);
                item.OfferLink = GetLink(item);
                if (item.Description?.Length > 50)
                    item.Description = item.Description.Substring(0, 50) + "...";
                item.Title = GetTitle(item);
            }
            
            CurrentOffers = offers.Items;
        }

        private string GetLink(AggregatedOfferDto item)
        {
            return item.OfferType switch
            {
                OfferType.Personal => "/Offer/" + item.Id,
                OfferType.Lobby => "/Lobby/" + item.Id,
                OfferType.Place => "/Place/" + item.Id,
                OfferType.Event => "/Event/" + item.Id,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string GetTitle(AggregatedOfferDto item)
        {
            return item.OfferType switch
            {
                OfferType.Personal => item.Title,
                OfferType.Lobby => item.Title ?? "Хотим поиграть",
                OfferType.Place => item.Title,
                OfferType.Event => item.Title,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private async Task<string> GetPosterAsync(AggregatedOfferDto item)
        {
            if (item.PosterUrl.IsNullOrWhiteSpace())
                return item.OfferType switch
                {
                    OfferType.Personal => await ApiClient.GetRandomOfferPictureLinkAsync(),
                    OfferType.Lobby => await ApiClient.GetRandomLobbyPictureLinkAsync(),
                    OfferType.Place => await ApiClient.GetRandomLobbyPictureLinkAsync(),
                    OfferType.Event => await ApiClient.GetRandomLobbyPictureLinkAsync(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            
            if (item.PosterUrl.Contains("http"))
                return item.PosterUrl;
                
            return ApiInfo.Value.Address + item.PosterUrl;


        }
    }
}