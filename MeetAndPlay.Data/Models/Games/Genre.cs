using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models.Games
{
    public class Genre : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
    }
}