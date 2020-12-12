using System;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Commons;

namespace MeetAndPlay.Data.Models.Offers
{
    public class UserOfferPeriod : PeriodBase
    {
        public Guid UserOfferId { get; set; }
        public virtual UserOffer UserOffer { get; set; }
    }
}