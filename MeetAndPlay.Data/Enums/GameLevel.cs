using System.ComponentModel;

namespace MeetAndPlay.Data.Enums
{
    public enum GameLevel
    {
        [Description("Не задано")]
        Undefined,
        [Description("Не знаю как играть")]
        Noob,
        [Description("Понимаю как играть, знаю правила")]
        Middle,
        [Description("Подержи моё пиво, сынок")]
        Pro
    }
}