using System;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Pages
{
    public class CategoryComponent : ComponentBase
    {
        [Parameter]
        public string OfferTypeName { get; set; }

        protected CategoryViewModel CurrentCategory { get; set; }
        protected CategoryViewModel[] OtherCategories { get; set; }
        protected OfferType OfferType => Enum.Parse<OfferType>(OfferTypeName);

        
        //TODO: Разобраться со State объекта
        protected override Task OnInitializedAsync()
        {
            var categories = CategoryViewModelsStorage.GetCategories().ToArray();
            CurrentCategory = categories.Single(c => c.OfferType == OfferType);
            OtherCategories = categories.Where(c => c.OfferType != OfferType).ToArray();
            return Task.CompletedTask;
        }
    }
}