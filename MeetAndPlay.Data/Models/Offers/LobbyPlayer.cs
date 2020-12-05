using System;

namespace MeetAndPlay.Data.Models.Offers
{
    public class LobbyPlayer
    {
        public Guid LobbyId { get; set; }
        public Guid PlayerId { get; set; }
        public bool IsCreator { get; set; }
    }
}