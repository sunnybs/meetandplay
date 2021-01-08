using System;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Games
{
    public class GameImage : FileWithCompressedCopy
    {
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
        public bool IsCurrentPoster { get; set; }
    }
}