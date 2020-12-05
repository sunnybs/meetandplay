using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Web.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public string PosterUrl { get; set; }
        public string Link => $"/category/{OfferType}";
        public OfferType OfferType { get; set; }
    }
}