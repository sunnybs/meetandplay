using System;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Offers
{
    public class LobbyImage
    {
        public Guid LobbyId { get; set; }
        public virtual Lobby Lobby { get; set; }
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
        public bool IsCurrentPoster { get; set; }
    }
}