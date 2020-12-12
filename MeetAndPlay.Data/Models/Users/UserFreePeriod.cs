using System;
using MeetAndPlay.Data.Models.Commons;

namespace MeetAndPlay.Data.Models.Users
{
    public class UserFreePeriod : PeriodBase
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}