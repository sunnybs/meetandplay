using System;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Data.Models.Offers
{
    public class UserOfferGame
    {
        public Guid UserOfferId { get; set; }
        public virtual UserOffer UserOffer { get; set; }
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}