using System;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Data.Models.Offers
{
    public class LobbyGame
    {
        public Guid LobbyId { get; set; }
        public virtual Lobby Lobby { get; set; }
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}