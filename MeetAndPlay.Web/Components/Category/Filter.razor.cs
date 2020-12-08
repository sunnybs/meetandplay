using System;
using System.Threading.Tasks;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MeetAndPlay.Web.Components.Category
{
    public class FilterComponent : ComponentBase
    {
        [Parameter] public string OfferTypeName { get; set; }
        private OfferType OfferType => Enum.Parse<OfferType>(OfferTypeName);

        protected ElementReference DateFilter;
        protected ElementReference GameFilter;
        protected ElementReference PlaceFilter;
        protected ElementReference PeopleFilter;

        private const string DefaultClasses = "p-3 mx-4 btn btn-warning bg-lightyellow border-0 rounded-pill";
        protected string DateFilterClasses;
        protected string GameFilterClasses;
        protected string PlaceFilterClasses;
        protected string PeopleFilterClasses;

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