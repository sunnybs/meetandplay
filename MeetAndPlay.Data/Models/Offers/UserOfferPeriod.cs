using System;

namespace MeetAndPlay.Data.Models.Offers
{
    public class UserOfferPeriod
    {
        public Guid UserOfferPeriodId { get; set; }
        public Guid UserOfferId { get; set; }
        public bool IsEveryday { get; set; }
        public bool IsDayOfWeek { get; set; }
        public WeekDays Day { get; set; }
        public int HoursFrom { get; set; }
        public int HoursTo { get; set; }
    }
}