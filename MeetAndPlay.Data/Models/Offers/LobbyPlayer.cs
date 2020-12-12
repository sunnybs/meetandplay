using System;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.Models.Offers
{
    public class LobbyPlayer
    {
        public Guid LobbyId { get; set; }
        public virtual Lobby Lobby { get; set; }
        public Guid PlayerId { get; set; }
        public virtual User Player { get; set; }
        public bool IsCreator { get; set; }
    }
}