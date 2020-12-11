using System;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.DTO.OfferAggregator
{
    public class OffersFilterDto
    {
        public OfferType? OfferType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string GameName { get; set; }
        public GameLevel? GameLevel { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public PlaceType? PlaceType { get; set; }
    }
}