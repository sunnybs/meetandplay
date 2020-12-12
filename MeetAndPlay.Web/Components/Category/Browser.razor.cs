using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO.OfferAggregator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MeetAndPlay.Web.Components.Category
{
    public class BrowserComponent : ComponentBase
    {
        [CascadingParameter] public OffersFilterDto FilterModel { get; set; }
        [Inject] public IOfferAggregator OfferAggregator { get; set; }
        protected AggregatedOfferDto[] CurrentOffers { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            FilterModel.StateHasChanged += async () =>
            {
                await UpdateOffersAsync();
                StateHasChanged();
            };
            await UpdateOffersAsync();
        }

        private async Task UpdateOffersAsync()
        {
            var offers = await OfferAggregator.AggregateOffersAsync(FilterModel);
            CurrentOffers = offers.Items;
        }
    }
}