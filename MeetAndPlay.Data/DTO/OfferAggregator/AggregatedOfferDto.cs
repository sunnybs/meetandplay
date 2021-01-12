using System;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Data.DTO.OfferAggregator
{
    public class AggregatedOfferDto
    {
        public Guid Id { get; set; }
        public OfferType OfferType{ get; set; }
        public PlaceType PlaceType { get; set; }
        public GameLevel GameLevel { get; set; }
        public string Title{ get; set; }
        public string Description{ get; set; }
        public string PosterUrl{ get; set; }
        public string[] AdditionalImages{ get; set; }
        public string[] Games { get; set; }
        public DateTime? ActualOfferDate { get; set; }
        public string OfferLink { get; set; }

        public AggregatedOfferDto(Guid id,
            OfferType offerType,
            string title,
            string description,
            string posterUrl,
            string[] additionalImages,
            DateTime? actualOfferDate,
            string[] games,
            GameLevel gameLevel,
            PlaceType placeType)
        {
            Id = id;
            OfferType = offerType;
            Title = title;
            Description = description;
            PosterUrl = posterUrl;
            AdditionalImages = additionalImages;
            ActualOfferDate = actualOfferDate;
            Games = games;
            GameLevel = gameLevel;
            PlaceType = placeType;
        }
    }
}