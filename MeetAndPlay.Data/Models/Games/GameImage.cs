using System;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Games
{
    public class GameImage
    {
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
        public bool IsCurrentPoster { get; set; }
    }
}