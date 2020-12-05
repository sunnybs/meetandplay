using System;
using System.Threading.Tasks;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Web.Services;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Components.Category
{
    public class FilterComponent : ComponentBase
    {
        [Parameter] public string OfferTypeName { get; set; }
        [Inject] private JSHelper JSHelper { get; set; }
        protected OfferType OfferType => Enum.Parse<OfferType>(OfferTypeName);

        protected ElementReference DateFilter;
        protected ElementReference GameFilter;
        protected ElementReference PlaceFilter;
        protected ElementReference PeopleFilter;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            switch (OfferType)
            {
                case OfferType.Event:
                {
                    await JSHelper.AddClassAsync(GameFilter, "d-none");
                    await JSHelper.AddClassAsync(PlaceFilter, "d-none");
                    await JSHelper.AddClassAsync(PeopleFilter, "d-none");
                    break;
                }
                case OfferType.Place:
                {
                    await JSHelper.AddClassAsync(PeopleFilter, "d-none");
                    break;
                }
            }
        }
    }
}