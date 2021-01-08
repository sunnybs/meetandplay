using System;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Offers
{
    public class LobbyImage : FileWithCompressedCopy
    {
        public Guid LobbyId { get; set; }
        public virtual Lobby Lobby { get; set; }
        public bool IsCurrentPoster { get; set; }
    }
}