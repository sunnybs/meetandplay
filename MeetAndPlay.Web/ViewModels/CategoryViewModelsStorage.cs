using System.Collections.Generic;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Web.ViewModels
{
    public static class CategoryViewModelsStorage
    {
        public static IEnumerable<CategoryViewModel> GetCategories()
        {
            yield return new CategoryViewModel
            {
                Name = "Игроки",
                PosterUrl = "/images/add.svg",
                OfferType = OfferType.Personal
            };
            yield return new CategoryViewModel
            {
                Name = "Компании",
                PosterUrl = "/images/teamwork.svg",
                OfferType = OfferType.Lobby
            };
            yield return new CategoryViewModel
            {
                Name = "Места",
                PosterUrl = "/images/place.svg",
                OfferType = OfferType.Place
            };
            yield return new CategoryViewModel
            {
                Name = "События",
                PosterUrl = "/images/calendar.svg",
                OfferType = OfferType.Event
            };
        }
    }
}