using System.ComponentModel;

namespace MeetAndPlay.Data.Enums
{
    public enum PlaceType
    {
        [Description("Не задано")]
        Undefined,
        [Description("Дома")]
        Home,
        [Description("В кафе")]
        Cafe,
        [Description("В лаундж баре")]
        Lounge,
    }
}