using System;
using System.Threading.Tasks;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MeetAndPlay.Web.Components.Category
{
    public class FilterComponent : ComponentBase
    {
        [Parameter] public string OfferTypeName { get; set; }
        protected OfferType OfferType => Enum.Parse<OfferType>(OfferTypeName);

        protected ElementReference DateFilter;
        protected ElementReference GameFilter;
        protected ElementReference PlaceFilter;
        protected ElementReference PeopleFilter;

        private const string DefaultClasses = "p-3 mx-4 btn btn-warning bg-lightyellow border-0 rounded-pill dropdown-toggle";
        protected const string DropdownMenuClasses = "dropdown-menu p-4 shadow border-0";
        
        protected string DateFilterClasses;
        protected string GameFilterClasses;
        protected string PlaceFilterClasses;
        protected string PeopleFilterClasses;

        [CascadingParameter] protected OffersFilterDto FilterModel { get; set; }

        protected override void OnInitialized()
        {
            SetDefaultClasses();
        }

        protected override Task OnParametersSetAsync()
        {
            SetDefaultClasses();
            switch (OfferType)
            {
                case OfferType.Place:
                    PeopleFilterClasses += " d-none";
                    break;
                case OfferType.Event:
                    GameFilterClasses += " d-none";
                    PlaceFilterClasses += " d-none";
                    PeopleFilterClasses += " d-none";
                    break;
            }
            return Task.CompletedTask;
        }

        private void SetDefaultClasses()
        {
            DateFilterClasses = DefaultClasses;
            GameFilterClasses = DefaultClasses;
            PlaceFilterClasses = DefaultClasses;
            PeopleFilterClasses = DefaultClasses;
        }
    }
}