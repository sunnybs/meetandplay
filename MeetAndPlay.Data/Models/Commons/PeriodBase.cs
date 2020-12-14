using System.ComponentModel.DataAnnotations.Schema;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.Models.Commons
{
    [NotMapped]
    public class PeriodBase : BaseEntity
    {
        public bool IsEveryday { get; set; }
        public bool IsDayOfWeek { get; set; }
        public WeekDays Day { get; set; }
        public int HoursFrom { get; set; }
        public int HoursTo { get; set; }
    }
}