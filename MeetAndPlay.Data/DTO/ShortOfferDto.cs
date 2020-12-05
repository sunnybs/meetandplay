using System;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.DTO
{
    public class ShortOfferDto
    {
        public Guid OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public DateTime Date { get; set; }
        public OfferType OfferType { get; set; }
    }
}