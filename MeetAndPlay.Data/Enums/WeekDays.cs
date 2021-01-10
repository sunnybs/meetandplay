using System.ComponentModel;

namespace MeetAndPlay.Data.Enums
{
    public enum WeekDays
    {
        [Description("Понедельник")]
        Monday,
        [Description("Вторник")]
        Tuesday,
        [Description("Среда")]
        Wednesday,
        [Description("Четверг")]
        Thursday,
        [Description("Пятница")]
        Friday,
        [Description("Суббота")]
        Saturday,
        [Description("Воскресенье")]
        Sunday
    }
}