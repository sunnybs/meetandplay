using System;

namespace MeetAndPlay.Offers.Data.Models
{
    public class LobbyPlayer
    {
        public Guid LobbyId { get; set; }
        public Guid PlayerId { get; set; }
        public bool IsCreator { get; set; }
    }
}