using System.ComponentModel;

namespace MeetAndPlay.Data.Enums
{
    public enum OfferType
    {
        [Description("Игроки")]
        Personal,
        [Description("Команды")]
        Lobby,
        [Description("Места")]
        Place,
        [Description("События")]
        Event
    }
}