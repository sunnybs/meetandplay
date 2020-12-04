using System.Collections.Generic;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Components
{
    public class CategoriesComponent : ComponentBase
    {
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            yield return new CategoryViewModel
            {
                Link = "/players",
                Name = "Игрок",
                PosterUrl = "/images/add.svg"
            };
            yield return new CategoryViewModel
            {
                Link = "/companies",
                Name = "Компания",
                PosterUrl = "/images/teamwork.svg"
            };
            yield return new CategoryViewModel
            {
                Link = "/places",
                Name = "Место",
                PosterUrl = "/images/place.svg"
            };
            yield return new CategoryViewModel
            {
                Link = "/events",
                Name = "События",
                PosterUrl = "/images/calendar.svg"
            };
        }
    }
}