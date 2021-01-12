using System;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Pages
{
    public class CategoryComponent : ComponentBase
    {
        [Parameter] public string OfferTypeName { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        
        [Inject] private ILobbyService LobbyService { get; set; }
        [Inject] private IUserOfferService UserOfferService { get; set; }
        [Inject] private IPlaceService PlaceService { get; set; }
        
        protected CategoryViewModel CurrentCategory { get; set; }
        protected CategoryViewModel[] OtherCategories { get; set; }
        protected OfferType OfferType => Enum.Parse<OfferType>(OfferTypeName);
        protected OffersFilterDto FilterModel = new();

        protected IOfferAggregator OfferAggregator { get; set; }

        protected override void OnInitialized()
        {
            OfferAggregator = OfferType switch
            {
                OfferType.Personal => UserOfferService,
                OfferType.Lobby => LobbyService,
                OfferType.Place => PlaceService,
                OfferType.Event => LobbyService,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        //TODO: Разобраться со State объекта
        protected override Task OnParametersSetAsync()
        {
            var categories = CategoryViewModelsStorage.GetCategories().ToArray();
            CurrentCategory = categories.Single(c => c.OfferType == OfferType);
            OtherCategories = categories.Where(c => c.OfferType != OfferType).ToArray();
            return Task.CompletedTask;
        }
    }
}